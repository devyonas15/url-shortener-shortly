import { Outlet, Navigate } from 'react-router-dom';
import { getSessionItem } from '../../utils/storageUtils/sessionStorageUtils';
import { SESSION_DATA } from '../../utils/constants/SessionStorageKey';
import { LoginResponse } from '../../../auth/login/types/LoginDTO';

const ProtectedRoute = () => {
  const userData = getSessionItem<LoginResponse>(SESSION_DATA, true);

  if (null === userData) {
    return <Navigate to='/login' />;
  }

  return <Outlet />;
};

export default ProtectedRoute;
