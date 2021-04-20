<template>
  <!--
    Copyright 2021 HGTP Capstone Team at the University of Utah: 
    Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
    Licensed under the MIT license. Read the project readme for details.
  -->
  <div class="home">
    <b-card-group deck>
    <!-- Can be replaced with whatever you want... -->
      <b-card deck border-variant="dark">
        <h3>The HGTP Capstone Team</h3>

        <b-media tag="li" class="mb-3">
          <template #aside>
            <img src="../assets/Harrison.png" width="64" alt="placeholder" />
          </template>
          <h5 class="mt-0 mb-1">Harrison Quick</h5>
          <p>
            Computer Science BS / Math Minor student aspiring to get into data
            science, who enjoys camping and working on bikes among other things!
          </p>
          <p>
            Some classes taken include: Machine Learning, Foundations of Data
            Analysis, Computer Graphics and Computer Networks.
          </p>
          <p>
            Experience and employment: Prospective employee everywhere; if
            you're in data science call me!
          </p>
        </b-media>

        <b-media tag="li" class="mb-3">
          <template #aside>
            <img src="../assets/Misha.png" width="64" alt="placeholder" />
          </template>
          <h5 class="mt-0 mb-1">Misha Griego</h5>
          <p>
            A student who likes programming Mobile Apps, foreign languages, arts
            and crafts, and trying new things out! I can't wait to travel and
            gain more experience!
          </p>
          <p>
            Some classes taken include: Operating Systems, Mobile App
            Programming, Web Software Architecture, Computer Graphics, and
            Computer Security.
          </p>
          <p>
            Experience and employment: A future employee of Publicis Sapient and
            past intern at Ivanti.
          </p>
        </b-media>

        <b-media tag="li" class="mb-3">
          <template #aside>
            <img src="../assets/Jacob.png" width="64" alt="placeholder" />
          </template>
          <h5 class="mt-0 mb-1">Jacob B. Norgaard</h5>
          <p>
            Majoring in Computer Science, Jacob is an individual that loves
            learning new things and travelling with his wife and son.
          </p>
          <p>
            Some classes taken include: Mobile App Programming, Computer
            Graphics, Parallel Programming and Optimized Computing, and Computer
            Security.
          </p>
          <p>
            Experience and employment: Full time Software Engineer.
          </p>
          <p>
            LinkedIn:
            <a href="www.linkedin.com/in/jacobbn">
              www.linkedin.com/in/jacobbn
            </a>
          </p>
        </b-media>

        <b-media tag="li">
          <template #aside>
            <img src="../assets/Emma.png" width="64" alt="placeholder" />
          </template>
          <h5 class="mt-0 mb-1">Emma Pinegar</h5>
          <p>
            Computer Science BS student who enjoys robotics, dogs, swimming, and
            reading! Emma is a member of the HGTP capstone team and an
            undergraduate researcher at the Learning Lab for Manipulation
            Autonomy. Upon the conclusion of the capstone project, she will be
            continuing her education at the University of Utah with graduate
            mechanical engineering and computer science courses with a focus on
            robotics.
          </p>
          <p>
            Some classes taken include: Robotics, Image Processing,
            Human/Computer Interaction, and Undergraduate Research.
          </p>
          <p>
            Experience and employment: Continuing undergraduate researcher at
            the LL4MA Lab funded by an NSF REU.
          </p>
          <p>
            Email: emma.k.pinegar@gmail.com <br />
            LinkedIn:
            <a href="https://www.linkedin.com/in/emma-pinegar/">
              www.linkedin.com/in/emma-pinegar
            </a>
          </p>
        </b-media>
      </b-card>

      <b-card deck border-variant="dark">
        <b-card-body
          title="The Product"
          sub-title="Gestr Companion App and Augmented Sense Technology"
        >
          <p class="card-text mt-3">
            Download the app here: COMING SOON...
          </p>
        </b-card-body>
        <img src="../assets/gestr-app.png" class="card-img" />
        <b-card-body title="The Sponsor" sub-title="Augmented Sense Technology">
          <p class="card-text mt-3">
            Check them out at
            <a href="https://www.augsensetech.com/">their website</a>
          </p>
        </b-card-body>
        <img
          src="../assets/Augmented_Sense_Technologies.png"
          class="card-img"
        />
      </b-card>
    </b-card-group>
  </div>
</template>

<script>
export default {
  name: 'Home',
};
</script>

<style scoped>
.card-img {
  position: relative;
  left: 50%;
  transform: translateX(-50%);
  width: 40%;
  margin: auto;
}
p {
  margin-top: 4px;
  margin-bottom: 4px;
}
.team-member-wrapper {
  width: 100%;
}
.team-member-image {
  float: left;
  top: 50%;
}
.team-member-image img {
  width: 102px;
}
.team-member-description {
  float: right;
  margin-left: 110px;
  padding: 12px;
}
.home {
  padding: 36px;
}
#portraits {
  position: relative;
  top: 0;
  left: 50%;
  max-width: calc(24vw + 12px);
  transform: translateX(-50%);
}
#portraits img {
  width: 6vw;
  height: 6vw;
  margin-left: 3px;
  border: 1px solid gray;
  border-radius: 50%;
}
</style>
