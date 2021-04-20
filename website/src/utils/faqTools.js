// Copyright 2021 HGTP Capstone Team at the University of Utah:
// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
// Licensed under the MIT license. Read the project readme for details.

export const getFAQs = () => {
  var faqs = [];
  faqs.push({
    title: 'How do I pair my Gestr Device to the Gestr App?',
    answer:
      'Go to the settings page and go to the pair device page.' +
      ' Pressing scan will allow the app to scan for available devices. ' +
      'The devices available will show below the button.' +
      'Click the device that needs to be paired. This will pair the device with the app.',
  });
  faqs.push({
    title: 'How do I Request a New Feature?',
    answer: 'Navigate to our request page: ADD LINK HERE',
  });
  faqs.push({
    title: 'How Do I set my Emergency Contacts?',
    answer:
      'This can be done on the profile page of the website or the mobile app.',
  });
  return faqs;
};
