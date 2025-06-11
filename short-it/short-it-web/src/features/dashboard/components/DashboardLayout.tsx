import { AppProvider, Session } from '@toolpad/core/AppProvider';
import { DashboardLayout } from '@toolpad/core/DashboardLayout';
import { PageContainer } from '@toolpad/core/PageContainer';
import CreateLinkPage from '../../url/create-url-link/pages/CreateLinkPage';
import { useRouter } from '../hooks/useRouter';
import { JSX, useMemo, useState } from 'react';
import { DashboardRoutes } from '../common/enums/dashboardEnums';
import { navigation } from '../common/configs/navigationConfig';
import { dashboardTheme } from './DashboardLayout.styles';
import useSessionAuth from '../hooks/useSessionAuth';

const getPageComponent = (pathName: string): JSX.Element | undefined => {
  switch (pathName) {
    case DashboardRoutes.Home:
    case DashboardRoutes.Analytics:
      break;
    case DashboardRoutes.Links:
      return <CreateLinkPage />;
    default:
      break;
  }
};

const Dashboard = () => {
  const router = useRouter(DashboardRoutes.Home);
  const { session, authentication } = useSessionAuth();

  return (
    <AppProvider
      navigation={navigation}
      authentication={authentication}
      session={session}
      router={router}
      theme={dashboardTheme}
      branding={{
        logo: <img src='/short-it.svg' alt='Short-it Logo' />,
        title: '',
        homeUrl: DashboardRoutes.Home,
      }}
    >
      <DashboardLayout>
        <PageContainer title=''>
          {getPageComponent(router.pathname)}
        </PageContainer>
      </DashboardLayout>
    </AppProvider>
  );
};

export default Dashboard;
