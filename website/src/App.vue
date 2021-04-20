<template>
  <!--
    Copyright 2021 HGTP Capstone Team at the University of Utah: 
    Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
    Licensed under the MIT license. Read the project readme for details.
  -->
  <div id="app">
    <!-- https://bootstrap-vue.org/docs/components/navbar#color-schemes -->
    <b-navbar
      id="top-navbar"
      class="border-bottom border-dark"
      type="light"
      variant="light"
      fixed
    >
      <b-navbar-brand to="/">
        <img
          alt="Gestr logo"
          id="gestr-logo"
          class="d-inline-block align-top"
          src="./assets/gestr-logo.webp"
        />
      </b-navbar-brand>
      <b-navbar-nav>
        <b-nav-item to="/">Home</b-nav-item>
        <b-nav-item to="/faq">Help</b-nav-item>
      </b-navbar-nav>
      <b-navbar-nav class="ml-auto">
        <b-nav-item href="#" v-if="!authenticated" @click="login()">
          Sign In
        </b-nav-item>
        <b-nav-item-dropdown v-if="authenticated" :text="user" right>
          <b-dropdown-item to="/profile">Profile</b-dropdown-item>
          <b-dropdown-item to="/gestures">Gesture Settings</b-dropdown-item>
          <b-dropdown-item to="/feature-requests">
            Feature Requests
          </b-dropdown-item>
          <b-dropdown-item href="#" @click="logout()">
            Sign Out
          </b-dropdown-item>
        </b-nav-item-dropdown>
      </b-navbar-nav>
    </b-navbar>
    <router-view />
  </div>
</template>

<script>
import axios from 'axios';

export default {
  name: 'App',
  data: function() {
    return {
      authenticated: false,
      user: 'User',
    };
  },
  async created() {
    if (!this.$auth.isLoginRedirect()) {
      this.isAuthenticated();
    }
  },
  watch: {
    $route: 'isAuthenticated',
  },
  methods: {
    login() {
      this.$router.push({ path: '/login' });
    },
    async logout() {
      await this.$auth.signOut();
      if (this.$route.name !== 'Home') {
        this.$router.push({ path: '/' });
      }
    },
    async isAuthenticated() {
      this.authenticated = await this.$auth.isAuthenticated();
      if (this.authenticated) {
        const user = await this.$auth.getUser();
        if (user && user.name) {
          this.user = user.name;
        }
        axios.defaults.headers.common['Authorization'] =
          'Bearer ' + (await this.$auth.getAccessToken());
      } else {
        axios.defaults.headers.common['Authorization'] = undefined;
        this.user = 'User';
      }
    },
  },
};
</script>

<style>
#top-navbar {
  height: 65px;
}
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  color: #2c3e50;
}
#gestr-logo {
  height: 38px;
}
.dropdown-menu .dropdown-item:focus {
  background-color: #2971a4;
}
</style>
