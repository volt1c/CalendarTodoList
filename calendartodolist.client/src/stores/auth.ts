import { createStore } from "vuex";

export type AuthStore = {
  accessToken: string;
  expiresIn: number;
  loginTime: number;
};

export const key = "auth";
const store = createStore({
  state: {
    accessToken: localStorage.getItem("accessToken") ?? "",
    expiresIn: Number(localStorage.getItem("expiresIn")) ?? 0,
    loginTime: Number(localStorage.getItem("loginTime")) ?? 0,
  },
  mutations: {
    storeLogin(state, payload: AuthStore) {
      state.accessToken = payload.accessToken;
      state.expiresIn = payload.expiresIn;
      state.loginTime = payload.loginTime;
    },
    storeClear(state) {
      state.accessToken = "";
      state.expiresIn = 0;
      state.loginTime = 0;
    },
  },
  getters: {
    isAuthorized(state) {
      if (!state.accessToken) return false;

      const expirationTime = state.loginTime + state.expiresIn * 1000;

      if (expirationTime < Date.now()) return false;

      return true;
    },
  },
});

export default store;
