import { Outlet, Navigate } from 'react-router-dom';
import { getSessionItem } from '../../utils/storageUtils/sessionStorageUtils';
import { BEARER_TOKEN_SESSION_NAME } from '../../utils/constants/SessionStorageKey';

const ProtectedRoute = () => {
  const authToken = getSessionItem(BEARER_TOKEN_SESSION_NAME);

  return authToken ? <Outlet /> : <Navigate to='/login' />;
};

export default ProtectedRoute;
