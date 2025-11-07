import { ReactNode } from 'react';
import { Navigate } from 'react-router-dom';
import { AuthenticationService } from '../../services/authentication-service';

interface AuthGuardProps {
  children: ReactNode;
  redirectTo?: string;
}

const authService = new AuthenticationService();

export default function AuthGuard({ children, redirectTo = '/home' }: AuthGuardProps) {
  if (authService.isAuthenticated()) {
    return <Navigate to={redirectTo} replace />;
  }

  return <>{children}</>;
}