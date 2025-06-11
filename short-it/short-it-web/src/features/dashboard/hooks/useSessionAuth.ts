import { Session } from "@toolpad/core";
import { useMemo, useState } from "react";
import { deleteSessionItem, getSessionItem } from "../../shared/utils/storageUtils/sessionStorageUtils";
import { SESSION_DATA } from "../../shared/utils/constants/SessionStorageKey";
import { LoginResponse } from "../../auth/login/types/LoginDTO";
import { useNavigate } from "react-router-dom";

const useSessionAuth = () => {
    const navigate = useNavigate();
    let userData = getSessionItem<LoginResponse>(SESSION_DATA, true);

    if (null !== userData) {
        userData = userData as LoginResponse;
    }

    const [session, setSession] = useState<Session | null>({
        user: {
          id: userData?.loginId,
          email: userData?.email,
        },
      });
    
    const authentication = useMemo(() => {
        return {
            signIn: () => {
            setSession({
                user: {
                    id: userData?.loginId,
                    email: userData?.email,
                },
            });
            },
            signOut: () => {
                setSession(null);
                deleteSessionItem(SESSION_DATA);
                navigate('/login');
            },
        };
    }, []);

    return {session, authentication}
}

export default useSessionAuth;