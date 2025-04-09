import { ReactNode } from 'react';

export interface NotificationProps {
  title?: string;
  message: string;
  severity: 'success' | 'info' | 'warning' | 'error';
  duration?: number;
  icon: ReactNode;
}
