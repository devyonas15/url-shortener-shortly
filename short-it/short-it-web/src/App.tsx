// Temporarily use css for body's background color
import './app.css';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Dashboard from './features/dashboard/components/DashboardLayout';
import LoginPage from './features/auth/login/pages/LoginPage';
import ProtectedRoute from './features/shared/components/route/ProtectedRoute';

const App = () => {
  return (
    <BrowserRouter>
      <Routes>
        <Route element={<LoginPage />} path='/login' />

        <Route element={<ProtectedRoute />}>
          <Route element={<Dashboard />} path='/' />
        </Route>
        <Route element={<Dashboard />} path='/' />
      </Routes>
    </BrowserRouter>
  );
};

export default App;
