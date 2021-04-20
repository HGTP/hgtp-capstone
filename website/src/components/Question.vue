<template>
  <!--
    Copyright 2021 HGTP Capstone Team at the University of Utah: 
    Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
    Licensed under the MIT license. Read the project readme for details.
  -->
  <div class="accordion mb-4" id="QandA" role="tablist">
    <b-card no-body class="mx-auto" style="width: 75%;" id="questions-card">
      <b-card-header header-tag="header" class="p-0">
        <b-button block @click="toggle()" variant="info" id="question-button">{{
          title
        }}</b-button>
      </b-card-header>
      <b-collapse v-model="isVisible">
        <b-card-body>
          <b-card-text>{{ answer }}</b-card-text>
        </b-card-body>
      </b-collapse>
    </b-card>
  </div>
</template>

<script>
export default {
  name: 'Question',
  props: {
    title: String,
    answer: String,
  },
  data: function() {
    return {
      isVisible: false,
    };
  },
  methods: {
    toggle() {
      this.isVisible = !this.isVisible;
    },
  },
};
</script>

<style scoped>
#question-button {
  font-family: Helvetica, Arial, sans-serif;
  text-align: center;
  font-size: x-large;
  height: 75px;
  background-color: #007bff;
}
</style>
