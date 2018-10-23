

export interface RootState {
    auth: AuthState,
}

export interface AuthState {
    token: string | null;
    returnUrl: string;
};

export interface LayoutState {
    public: boolean;
    navbarDrawer: boolean;
}



