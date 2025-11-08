import React, { useEffect } from 'react';
import { Navigate, useLocation } from 'react-router-dom';
import { useProfile } from '../../hooks/useProfile';
import ProfileForm from './ProfileForm';

interface ProfileGuardProps {
  children: React.ReactNode;
}

const ProfileGuard: React.FC<ProfileGuardProps> = ({ children }) => {
  const { hasProfile, loading, checkProfile } = useProfile();
  const location = useLocation();

  useEffect(() => {
    checkProfile();
  }, [checkProfile]);

  if (loading) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <div className="animate-spin rounded-full h-32 w-32 border-b-2 border-blue-600"></div>
      </div>
    );
  }

  if (!hasProfile) {
    // If no profile, show profile creation form
    return (
      <div className="min-h-screen bg-gray-50 flex items-center justify-center p-4">
        <ProfileForm
          mode="create"
          onSuccess={() => {
            // After successful creation, the guard will re-check and allow access
            window.location.reload();
          }}
        />
      </div>
    );
  }

  // If profile exists, render the protected content
  return <>{children}</>;
};

export default ProfileGuard;