import { inject } from '@angular/core';
import { HttpInterceptorFn, HttpErrorResponse, HttpRequest, HttpHandlerFn } from '@angular/common/http';
import { map, Observable, of, tap, throwError } from 'rxjs';
import { catchError, switchMap, filter, take } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AuthService } from './auth.service';

export const AuthInterceptor: HttpInterceptorFn = (req: HttpRequest<unknown>, next: HttpHandlerFn) => {
  const authService = inject(AuthService);
  const jwtService = inject(JwtHelperService);
  let clonedReq = req;

  // 1. Attach the access token if available
  const token = authService.getToken();
  if (token) {
    clonedReq = clonedReq.clone({
      setHeaders: { Authorization: `Bearer ${authService.getToken()}` }
    });
  };

  // Helper function to process token refresh logic and safely queue requests
  const handle401Error = (req: HttpRequest<unknown>, next: HttpHandlerFn, authService: AuthService) => {
    // If a refresh is already in progress, wait for the new token
    if (authService.isRefreshing$.value) {
      return authService.refreshTokenSubject$.pipe(
        filter(token => token !== null),
        take(1),
        switchMap((token) => next(req.clone({
          setHeaders: { Authorization: `Bearer ${token}` }
        })))
      );
    }

    // Block other incoming parallel requests from firing their own refresh call
    authService.isRefreshing$.next(true);
    authService.refreshTokenSubject$.next(null);

    // Trigger the refresh token call
    return authService.refreshToken().pipe(
      switchMap((res) => {
        authService.isRefreshing$.next(false);
        authService.refreshTokenSubject$.next(res.token);
        console.log('Token is refreshed');

        // Retry the original request with the new access token
        return next(req.clone({
          setHeaders: { Authorization: `Bearer ${res.token}` }
        }));
      }),
      catchError((refreshError) => {
        // the refresh token itself is expired or invalid
        console.log('refresh token is expired');
        authService.isRefreshing$.next(false);
        return throwError(() => refreshError);
      })
    );
  };

  // 2. Intercept responses and catch 401 Unauthorized errors
  return next(clonedReq).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401 && !clonedReq.url.includes('refresh-token')) {
        return handle401Error(clonedReq, next, authService);
      }
      return throwError(() => error);
    })
  );
};
