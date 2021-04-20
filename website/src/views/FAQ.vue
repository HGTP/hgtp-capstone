<template>
  <!--
    Copyright 2021 HGTP Capstone Team at the University of Utah: 
    Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
    Licensed under the MIT license. Read the project readme for details.
  -->
  <div class="home">
    <h2 class="text-center mt-4 mb-5">FAQ</h2>
    <div id="all-questions" v-for="question in QA" :key="question.title">
      <Question v-bind="question" class="question"></Question>
    </div>

    <div class="text-center">
      <h2>Still have Questions?</h2>
      <!--How to do an email: 
      https://forum.vuejs.org/t/how-to-set-up-a-mailto-href-vue-2-0/2001/2 -->
      <b-button
        variant="primary"
        size="lg"
        :href="'mailto:' + hgtpEmail"
        id="contact-us-button"
      >
        Contact Us</b-button
      >
    </div>
  </div>
</template>

<script>
import Question from '@/components/Question.vue';
import { getFAQs } from '@/utils/faqTools.js';
export default {
  name: 'Home',
  components: {
    Question,
  },
  created() {
    this.QA = getFAQs();
  },

  data: function() {
    return {
      // List of questions and answers
      QA: [],
      hgtpEmail: 'hgtp.capstone@gmail.com',
    };
  },
};
</script>
