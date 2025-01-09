import { inject } from "@angular/core";
import { CanActivateFn, Router } from "@angular/router";

export const AuthGuard: CanActivateFn = (route, state) => {
  const role = localStorage.getItem('Role');
  const router = inject(Router);

  console.log("Role:", role);

  if (role === 'User' || role === 'Admin') {
    return true;
  }

  console.log("User not authenticated, redirecting to login");
  router.navigate(['/auth/login']);
  return false; 
};
