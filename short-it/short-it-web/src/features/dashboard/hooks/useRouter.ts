import { Router } from '@toolpad/core';
import { useMemo, useState } from 'react';

export const useRouter = (initialPath: string): Router => {
  const [pathname, setPathname] = useState<string>(initialPath);

  const router = useMemo(() => {
    return {
      pathname,
      searchParams: new URLSearchParams(),
      navigate: (path: string | URL) => setPathname(String(path)),
    };
  }, [pathname]);

  return router;
};
