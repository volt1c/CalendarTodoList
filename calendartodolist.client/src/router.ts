import { createRouter, createWebHistory } from "vue-router";

const routes = [
  { path: "/", name: "Home", component: () => import("@/views/HomeView.vue") },
  {
    component: () => import("@/views/LoginView.vue"),
    path: "/login",
    name: "Login",
  },
  {
    path: "/register",
    name: "Register",
    component: () => import("@/views/RegisterView.vue"),
  },
  {
    path: "/calendar",
    name: "Calendar",
    component: () => import("@/views/CalendarView.vue"),
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
