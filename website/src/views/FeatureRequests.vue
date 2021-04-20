<template>
  <!--
    Copyright 2021 HGTP Capstone Team at the University of Utah: 
    Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
    Licensed under the MIT license. Read the project readme for details.
  -->
  <div id="feature-requests">
    <b-card no-body>
      <b-tabs nav-wrapper-class="w-25" vertical pills card lazy>
        <b-tab v-if="isAdmin" title="Completed Features">
          <CompletedFeatures v-bind:isAdmin="isAdmin"></CompletedFeatures>
        </b-tab>
        <b-tab title="Planned Features">
          <PlannedFeatures v-bind:isAdmin="isAdmin"></PlannedFeatures>
        </b-tab>
        <b-tab title="Requested Features">
          <RequestedFeatures v-bind:isAdmin="isAdmin"></RequestedFeatures>
        </b-tab>
        <b-tab title="Make a New Request">
          <FeatureRequestForm v-bind:isAdmin="isAdmin"></FeatureRequestForm>
        </b-tab>
      </b-tabs>
    </b-card>
  </div>
</template>

<script>
import CompletedFeatures from '@/components/CompletedFeatures.vue';
import FeatureRequestForm from '@/components/FeatureRequestForm.vue';
import PlannedFeatures from '@/components/PlannedFeatures.vue';
import RequestedFeatures from '@/components/RequestedFeatures.vue';
import { isAdmin } from '../utils/authorizationTools';

export default {
  components: {
    CompletedFeatures,
    FeatureRequestForm,
    PlannedFeatures,
    RequestedFeatures,
  },
  async created() {
    this.isAdmin = await isAdmin();
    console.log(this.isAdmin);
  },
  data: function() {
    return {
      activeTab: 0,
      isAdmin: false,
    };
  },
  methods: {
    setActive(newActiveTab) {
      this.activeTab = newActiveTab;
    },
  },
};
</script>

<style scoped>
#feature-requests {
  position: relative;
  top: 0;
  left: 0;
  /* 65px is height of #top-navbar */
  height: calc(100vh - 65px);
  width: 100vw;
  background-color: #f8f9fa;
  padding: 24px;
}
.card {
  border: 1px solid black;
}
</style>
