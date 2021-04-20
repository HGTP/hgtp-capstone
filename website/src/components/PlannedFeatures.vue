<template>
  <!--
    Copyright 2021 HGTP Capstone Team at the University of Utah: 
    Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
    Licensed under the MIT license. Read the project readme for details.
  -->
  <div>
    <h3>Planned Features</h3>
    <ul id="features">
      <li v-for="feature in paginatedFeatures" :key="feature.id">
        <Feature
          v-on:remove-feature-from-list="removeFeature"
          v-bind:isAdmin="isAdmin"
          v-bind="feature"
          @click="selectFeature(feature)"
        ></Feature>
      </li>
    </ul>
    <b-pagination
      v-model="currentPage"
      align="center"
      aria-controls="features"
      :total-rows="features.length"
      :per-page="featuresPerPage"
      first-text="First"
      prev-text="Prev"
      next-text="Next"
      last-text="Last"
      pills
    ></b-pagination>
  </div>
</template>

<script>
import Feature from '@/components/Feature.vue';
import { getPlannedFeatures } from '../utils/featureTools';

export default {
  components: {
    Feature,
  },
  async created() {
    this.features = await getPlannedFeatures();
  },
  data: function() {
    return {
      currentPage: 1,
      featuresPerPage: 6,
      features: [],
    };
  },
  computed: {
    paginatedFeatures() {
      const startPos = (this.currentPage - 1) * this.featuresPerPage;
      const endPos = this.currentPage * this.featuresPerPage;
      return this.features.slice(startPos, endPos);
    },
  },
  methods: {
    removeFeature(featureRequestId) {
      for (let i = 0; i < this.features.length; i++) {
        if (this.features[i].id === featureRequestId) {
          this.features.splice(i, 1);
        }
      }
    },
  },
  props: {
    isAdmin: Boolean,
  },
};
</script>

<style scoped>
#features {
  list-style-type: none;
}
h3 {
  text-align: center;
}
</style>
