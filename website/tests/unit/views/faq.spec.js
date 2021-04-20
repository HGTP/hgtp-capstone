import { mount } from '@vue/test-utils';
import FAQ from '@/views/FAQ.vue';
import { getFAQs } from '@/utils/faqTools.js';

describe('views/FAQ.vue', () => {
  let wrapper;

  beforeAll(() => {
    wrapper = mount(FAQ, {
      stubs: [
        'b-button',
        'b-card-body',
        'b-card-text',
        'b-collapse',
        'b-card',
        'b-card-header',
      ],
    });
  });

  /*comparing one by one because trying to compare all of the
   * props at once only results in an undefined result
   */
  it('props are passed in correctly', () => {
    expect(wrapper.vm.hgtpEmail).toBe('hgtp.capstone@gmail.com');
    expect(wrapper.vm.QA).toEqual(getFAQs());
  });

  /**
   * testing Dom attributes
   * https://reactgo.com/vue-testing-dom-attributes/
   */
  it('contact us button test', () => {
    const button = wrapper.find('#contact-us-button');
    expect(button.attributes().href).toBe('mailto:hgtp.capstone@gmail.com');
  });
});
