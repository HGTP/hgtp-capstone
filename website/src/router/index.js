import Vue from 'vue';
import VueRouter from 'vue-router';
import Home from '../views/Home.vue';
import Login from '../components/Login.vue';
import OktaVue from '@okta/okta-vue';
import { OktaAuth } from '@okta/okta-auth-js';
import authConfig from '../authConfig';

const oktaAuth = new OktaAuth(authConfig);

// Uses route level code-splitting. This generates a separate chunk
// for the route (for example gestures.[hash].js), which is lazy-loaded
// when the route is visited.
const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home,
  },
  {
    path: '/login',
    component: Login,
  },
  {
    path: '/feature-requests',
    name: 'Feature Requests',
    component: () =>
      import(
        /* webpackChunkName: "feature-requests" */ '../views/FeatureRequests.vue'
      ),
    meta: {
      requiresAuth: true,
    },
  },
  {
    path: '/gestures',
    name: 'Gestures',
    component: () =>
      import(/* webpackChunkName: "gestures" */ '../views/GestureSettings.vue'),
    meta: {
      requiresAuth: true,
    },
  },
  {
    path: '/profile',
    name: 'Profile',
    component: () =>
      import(/* webpackChunkName: "profile" */ '../views/Profile.vue'),
  },
  {
    path: '/faq',
    name: 'FAQ',
    component: () => import(/* webpackChunkName: "faq" */ '../views/FAQ.vue'),
  },
];

Vue.use(VueRouter);
Vue.use(OktaVue, { oktaAuth });

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes,
});

export default router;
