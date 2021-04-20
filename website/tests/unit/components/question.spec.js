import { shallowMount } from '@vue/test-utils';
import Question from '@/components/Question.vue';

describe('components/Question.vue', () => {
  const propsData = {
    title: 'Question1',
    answer: 'Answer1',
    isVisible: false,
  };
  let wrapper;

  beforeAll(() => {
    wrapper = shallowMount(Question, {
      propsData,
      stubs: [
        'b-card',
        'b-button',
        'b-card-body',
        'b-card-text',
        'b-collapse',
        'b-card-header',
      ],
    });
  });

  it('data is passed in correctly', () => {
    expect(wrapper.vm.title).toEqual('Question1');
  });

  it('visibility can change', async () => {
    expect(wrapper.vm.isVisible).toEqual(false);
    await wrapper.find('#question-button').trigger('click');
    expect(wrapper.emitted()).toBeTruthy();

    //call method for the wrapper
    await wrapper.vm.toggle();

    expect(wrapper.vm.isVisible).toEqual(true);
  });
});
