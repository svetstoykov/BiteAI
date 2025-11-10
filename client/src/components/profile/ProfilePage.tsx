import React, { useState } from 'react';
import ProfileView from './ProfileView';
import ProfileForm from './ProfileForm';
import { useProfile } from '../../hooks/useProfile';

const ProfilePage: React.FC = () => {
  const { profile, loading, error } = useProfile();
  const [isEditing, setIsEditing] = useState(false);

  if (loading) {
    return (
      <div className="flex justify-center items-center min-h-screen">
        <div className="animate-spin rounded-full h-32 w-32 border-b-2 border-blue-600"></div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="flex justify-center items-center min-h-screen">
        <div className="text-red-600 text-center">
          <h2 className="text-2xl font-bold mb-4">Error Loading Profile</h2>
          <p>{error}</p>
        </div>
      </div>
    );
  }

  if (!profile) {
    return (
      <div className="flex justify-center items-center min-h-screen">
        <div className="text-center">
          <h2 className="text-2xl font-bold mb-4">No Profile Found</h2>
          <p className="mb-4">You need to create a profile to continue.</p>
          <ProfileForm mode="create" onSuccess={() => setIsEditing(false)} />
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50 py-8">
      <div className="container mx-auto px-4">
        {isEditing ? (
          <ProfileForm
            mode="edit"
            onSuccess={() => setIsEditing(false)}
            onCancel={() => setIsEditing(false)}
          />
        ) : (
          <ProfileView profile={profile} onEdit={() => setIsEditing(true)} />
        )}
      </div>
    </div>
  );
};

export default ProfilePage;