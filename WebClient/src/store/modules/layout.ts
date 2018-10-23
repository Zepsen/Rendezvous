
import { LayoutState, RootState } from '../models/types';
import { GetterTree, MutationTree, ActionTree, Module } from 'vuex';

const state: LayoutState = {
    public: false,
    navbarDrawer: false,
};

const getters: GetterTree<LayoutState, RootState> = {
    isPublicPage: (state) : boolean => state.public,
    getNavDrawer: (state) : boolean => state.navbarDrawer,
};

const mutations: MutationTree<LayoutState> = {
    setPublic: (state, value) : void => state.public = value || false,
    toggleNavDrawer: (state) : void => { state.navbarDrawer = !state.navbarDrawer },
};

const actions: ActionTree<LayoutState, RootState> = {
    changePublic: ({commit}, value) : void => commit('setPublic', value),
};

export const layout: Module<LayoutState, RootState> = {
    namespaced: true,
    state,
    getters,
    mutations,
    actions,
}