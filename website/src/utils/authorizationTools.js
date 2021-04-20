// Copyright 2021 HGTP Capstone Team at the University of Utah:
// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
// Licensed under the MIT license. Read the project readme for details.

import axios from 'axios';

export const isAdmin = async () => {
  const result = await axios.get('/authorization/admin');
  return result.data;
};
