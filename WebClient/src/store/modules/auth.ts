
import { AuthState, RootState } from '../models/types';
import { GetterTree, MutationTree, ActionTree, Module } from 'vuex';
import { includes } from 'lodash';

const TOKEN: string = 'token';
const ROLESKEY: string = 'roles';

const state: AuthState = {
    token: localStorage.getItem(TOKEN),
    returnUrl: '/',
};

const getters: GetterTree<AuthState, RootState> = {
    getToken: (state) : string | null => state.token,
    

    getReturnUrl(state) {
        return state.returnUrl;
    },
};

const mutations: MutationTree<AuthState> = {
    setLoginData(state, data: any) {
        state.token = data.token;
        state.returnUrl = data.returnUrl || '/';

        localStorage.setItem(TOKEN, data.token);
        localStorage.setItem(ROLESKEY, data.roles);
    },

    cleanData(state) {
        state.token = null;
        state.returnUrl = '/';
        localStorage.removeItem(TOKEN);
        localStorage.removeItem(ROLESKEY);
    },
};

const actions: ActionTree<AuthState, RootState> = {
    login({ commit, rootState }, data: any): void {
        commit('setLoginData', data);
    },

    logout({ commit }): void {
        commit('cleanData');
    },
};


export const auth: Module<AuthState, RootState> = {
    namespaced: true,
    state,
    getters,
    mutations,
    actions,
};

