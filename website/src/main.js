import axios from 'axios';
axios.defaults.headers.post['Content-Type'] = 'application/json';
if (location.hostname === 'localhost') {
  axios.defaults.baseURL = 'http://localhost:8080'; // API localhost base url.
} else {
  axios.defaults.baseURL = ''; // API non-localhost base url.
}

import Vue from 'vue';
import App from './App.vue';
import router from './router';
import { BootstrapVue, IconsPlugin } from 'bootstrap-vue';
import { library } from '@fortawesome/fontawesome-svg-core';
import {
  faComments,
  faEdit,
  faForward,
  faLocationArrow,
  faMapMarkedAlt,
  faMoon,
  faMusic,
  faPause,
  faPhone,
  faPhoneSlash,
  faPlay,
  faPlusCircle,
  faQuestion,
  faSearch,
  faSms,
  faThumbsUp,
  faTrash,
  faVolumeDown,
  faVolumeMute,
  faVolumeUp,
} from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';

library.add(faComments);
library.add(faEdit);
library.add(faForward);
library.add(faLocationArrow);
library.add(faMapMarkedAlt);
library.add(faMoon);
library.add(faMusic);
library.add(faPause);
library.add(faPhone);
library.add(faPhoneSlash);
library.add(faPlay);
library.add(faPlusCircle);
library.add(faQuestion);
library.add(faSearch);
library.add(faSms);
library.add(faTrash);
library.add(faThumbsUp);
library.add(faVolumeDown);
library.add(faVolumeMute);
library.add(faVolumeUp);
Vue.component('font-awesome-icon', FontAwesomeIcon);

Vue.use(BootstrapVue);
Vue.use(IconsPlugin);

// Import Bootstrap an BootstrapVue CSS files (order is important)
import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap-vue/dist/bootstrap-vue.css';

Vue.config.productionTip = false;

new Vue({
  router,
  render: (h) => h(App),
}).$mount('#app');
