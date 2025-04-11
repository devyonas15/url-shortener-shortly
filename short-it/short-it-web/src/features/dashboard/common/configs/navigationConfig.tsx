import HomeIcon from '@mui/icons-material/Home';
import LinkIcon from '@mui/icons-material/Link';
import BarChartRoundedIcon from '@mui/icons-material/BarChartRounded';
import { Navigation } from '@toolpad/core/AppProvider';

export const navigation: Navigation = [
  {
    segment: 'home',
    title: 'Home',
    icon: <HomeIcon />,
  },
  {
    segment: 'links',
    title: 'Links',
    icon: <LinkIcon />,
  },
  {
    segment: 'analytics',
    title: 'Analytics',
    icon: <BarChartRoundedIcon />,
  },
];
