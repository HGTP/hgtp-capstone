<template>
  <!--
    Copyright 2021 HGTP Capstone Team at the University of Utah: 
    Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
    Licensed under the MIT license. Read the project readme for details.
  -->
  <div>
    <h3>Request a Feature</h3>

    <b-container fluid>
      <p>
        <!-- ignore-prettier -->
        When you've found a feature we can add for you,
        <!-- ignore-prettier -->
        please first check if someone has already requested it.
        <!-- ignore-prettier -->
        If they have, upvote that feature. If they haven't,
        <!-- ignore-prettier -->
        go ahead and request it here.
      </p>
      <b-row class="mt-2">
        <b-col sm="2">
          <label for="feature-search">Search existing features:</label>
        </b-col>
        <b-col sm="10">
          <b-input-group>
            <template #prepend>
              <b-input-group-text>
                <font-awesome-icon icon="search" size="1x" />
              </b-input-group-text>
            </template>
            <b-form-input
              id="feature-search"
              type="search"
              placeholder="Type here to search for existing features."
              disabled
            ></b-form-input>
          </b-input-group>
        </b-col>
      </b-row>

      <hr />

      <b-form-group>
        <b-row class="mt-2">
          <b-col sm="2">
            <label for="description">Description:</label>
          </b-col>
          <b-col sm="10">
            <b-form-textarea
              id="description"
              v-model="description"
              placeholder="Put the feature description here."
            ></b-form-textarea>
          </b-col>
        </b-row>

        <b-row class="mt-2">
          <b-col sm="2">
            <label for="tags">Tags:</label>
          </b-col>
          <b-col sm="10">
            <b-form-tags
              input-id="tags"
              v-model="tags"
              placeholder="Add tags by typing and hitting enter."
            ></b-form-tags>
          </b-col>
        </b-row>

        <b-row class="mt-2">
          <b-col sm="2">
            <label for="may-contact">
              Can we contact you if we have questions?
            </label>
          </b-col>
          <b-col sm="10">
            <b-form-checkbox
              id="may-contact"
              v-model="mayContact"
              name="checkbox-1"
              value="accepted"
            ></b-form-checkbox>
          </b-col>
        </b-row>
      </b-form-group>
      <b-spinner
        v-bind:class="{
          'float-right': true,
          'inactive-spinner': !isLoading,
        }"
        variant="success"
        label="Loading..."
      ></b-spinner>
      <b-button
        class="float-right mr-2 mb-4"
        variant="primary"
        @click="submitRequest()"
      >
        Submit
      </b-button>
    </b-container>
  </div>
</template>

<script>
import { requestFeature } from '../utils/featureTools';

export default {
  data() {
    return {
      description: '',
      isLoading: false,
      mayContact: false,
      tags: [],
    };
  },
  methods: {
    async submitRequest() {
      this.isLoading = true;
      await requestFeature({
        description: this.description,
        mayContact: this.mayContact,
        tags: this.tags,
      });
      this.tags.splice(0, this.tags.length);
      this.mayContact = false;
      this.isLoading = false;
    },
  },
};
</script>

<style scoped>
h3 {
  text-align: center;
}
.inactive-spinner {
  display: none;
}
</style>
