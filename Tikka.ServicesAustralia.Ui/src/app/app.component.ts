import { Component, inject, signal } from '@angular/core';
import { MediaMatcher } from '@angular/cdk/layout';
import { Router } from '@angular/router';
import { tap } from 'rxjs/operators';
import { AuthService } from './public/services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'Tikka Portal';
  showFiller = false;

  protected readonly isMobile = signal(true);

  private readonly _mobileQuery: MediaQueryList;
  private readonly _mobileQueryListener: () => void;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {
    const media = inject(MediaMatcher);

    this._mobileQuery = media.matchMedia('(max-width: 600px)');
    this.isMobile.set(this._mobileQuery.matches);
    this._mobileQueryListener = () => this.isMobile.set(this._mobileQuery.matches);
    this._mobileQuery.addEventListener('change', this._mobileQueryListener);
  }

  isUserLoggedIn() {
    if (this.authService.getToken()) return true;
    return false;
  }

  logout() {
    this.authService.logOut().pipe(
      // route to protected/dashboard, if logout was successfull
      tap(() => this.router.navigate(['']))
    ).subscribe();
  }
}
