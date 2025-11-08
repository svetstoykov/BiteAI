import { useState, useEffect, useCallback } from 'react';
import { UserProfile, CreateUserProfile, UpdateUserProfile } from '../models/profile';
import { ProfileService } from '../services/profile-service';

interface UseProfileState {
  profile: UserProfile | null;
  loading: boolean;
  error: string | null;
  hasProfile: boolean;
}

export const useProfile = () => {
  const [state, setState] = useState<UseProfileState>({
    profile: null,
    loading: false,
    error: null,
    hasProfile: false,
  });

  const checkProfile = useCallback(async () => {
    setState(prev => ({ ...prev, loading: true, error: null }));
    try {
      const result = await ProfileService.hasProfile();
      if (result.success) {
        setState(prev => ({ ...prev, hasProfile: result.data!, loading: false }));
      } else {
        setState(prev => ({ ...prev, error: result.message, loading: false }));
      }
    } catch (error) {
      setState(prev => ({ ...prev, error: 'Failed to check profile existence', loading: false }));
    }
  }, []);

  const fetchProfile = useCallback(async () => {
    setState(prev => ({ ...prev, loading: true, error: null }));
    try {
      const result = await ProfileService.getMyProfile();
      if (result.success) {
        setState(prev => ({
          ...prev,
          profile: result.data!,
          hasProfile: true,
          loading: false
        }));
      } else {
        setState(prev => ({ ...prev, error: result.message, loading: false }));
      }
    } catch (error) {
      setState(prev => ({ ...prev, error: 'Failed to fetch profile', loading: false }));
    }
  }, []);

  const createProfile = useCallback(async (data: CreateUserProfile) => {
    setState(prev => ({ ...prev, loading: true, error: null }));
    try {
      const result = await ProfileService.createProfile(data);
      if (result.success) {
        setState(prev => ({
          ...prev,
          profile: result.data!,
          hasProfile: true,
          loading: false
        }));
        return { success: true };
      } else {
        setState(prev => ({ ...prev, error: result.message, loading: false }));
        return { success: false, error: result.message };
      }
    } catch (error) {
      const errorMessage = 'Failed to create profile';
      setState(prev => ({ ...prev, error: errorMessage, loading: false }));
      return { success: false, error: errorMessage };
    }
  }, []);

  const updateProfile = useCallback(async (data: UpdateUserProfile) => {
    setState(prev => ({ ...prev, loading: true, error: null }));
    try {
      const result = await ProfileService.updateProfile(data);
      if (result.success) {
        setState(prev => ({
          ...prev,
          profile: result.data!,
          loading: false
        }));
        return { success: true };
      } else {
        setState(prev => ({ ...prev, error: result.message, loading: false }));
        return { success: false, error: result.message };
      }
    } catch (error) {
      const errorMessage = 'Failed to update profile';
      setState(prev => ({ ...prev, error: errorMessage, loading: false }));
      return { success: false, error: errorMessage };
    }
  }, []);

  const deleteProfile = useCallback(async () => {
    setState(prev => ({ ...prev, loading: true, error: null }));
    try {
      const result = await ProfileService.deleteProfile();
      if (result.success) {
        setState(prev => ({
          ...prev,
          profile: null,
          hasProfile: false,
          loading: false
        }));
        return { success: true };
      } else {
        setState(prev => ({ ...prev, error: result.message, loading: false }));
        return { success: false, error: result.message };
      }
    } catch (error) {
      const errorMessage = 'Failed to delete profile';
      setState(prev => ({ ...prev, error: errorMessage, loading: false }));
      return { success: false, error: errorMessage };
    }
  }, []);

  useEffect(() => {
    checkProfile();
  }, [checkProfile]);

  return {
    ...state,
    fetchProfile,
    createProfile,
    updateProfile,
    deleteProfile,
    checkProfile,
  };
};