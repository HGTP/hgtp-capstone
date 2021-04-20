'use strict';

const sinon = require('sinon');
const proxyquire = require('proxyquire').noCallThru();
const expect = require('chai').expect;

describe('services -> gesturePresets', () => {
  let service;
  let sandbox;

  let gestureFindAllStub;
  let gestureUpsertStub;
  const gestureSettingMock = {
    findAll: (...values) => gestureFindAllStub(...values),
    upsert: (...values) =>  gestureUpsertStub(...values),
  };

  let presetFindOneStub;
  const presetMock = {
    findOne: (...values) => presetFindOneStub(...values),
  };

  let literalStub;

  const mockUserId = 'userId';
  const mockGesture = 'mockGesture';
  const mockPhoneAction = 'mockPhoneAction';
  const mockPreset = {
    id: 'mockPresetId',
    name: 'mockPresetName',
  };
  
  before(() => {
    sandbox = sinon.createSandbox();
    gestureFindAllStub = sandbox.stub();
    gestureUpsertStub = sandbox.stub();
    presetFindOneStub = sandbox.stub();
    literalStub = sandbox.stub();

    service = proxyquire('../../../src/services/gesturePresets', {
      '../../models': {
        sequelize: {
          GestureSetting: gestureSettingMock,
          literal: (...values) => literalStub(...values),
          Preset: presetMock,
        },
      },
    });
  });

  afterEach(() => {
    sandbox.resetHistory();
  });

  after(() => {
    sandbox.restore();
  });

  describe('deleteGestureSetting', () => {
    beforeEach(() => {
      presetFindOneStub.resolves({id: mockPreset.id});
    });

    it('resets the phone action', async () => {
      await service.deleteGestureSetting(mockUserId, {
        gesture: mockGesture,
        presetName: mockPreset.name,
      });
      expect(presetFindOneStub.calledOnce).to.be.true;
      expect(gestureUpsertStub.calledOnceWithExactly({
        gesture: mockGesture,
        phoneAction: null,
        presetId: mockPreset.id,
      }, {
        fields: ['phoneAction'],
      })).to.be.true;
    });
  });
  
  describe('getGestureSetting', () => {
    it('returns phone action', async () => {
      gestureFindAllStub.resolves([{phoneAction: mockPhoneAction}]);
      const result = await service.getGestureSetting(mockUserId, {
        gesture: mockGesture,
        presetName: mockPreset.name,
      });
      expect(gestureFindAllStub.calledOnce).to.be.true;
      expect(result).to.equal(mockPhoneAction);
    });
  });

  describe('getUserGestureSettings', () => {
    beforeEach(() => {
      gestureFindAllStub.resolves([
        {
          gesture: `${mockGesture}-1`,
          phoneAction: `${mockPhoneAction}-1`,
          Preset: {
            name: `${mockPreset.name}-1`,
          },
        },
        {
          gesture: `${mockGesture}-2`,
          phoneAction: `${mockPhoneAction}-2`,
          Preset: {
            name: `${mockPreset.name}-2`,
          },
        },
      ]);
    });

    it('returns a list of gesture settings', async () => {
      const response = await service.getUserGestureSettings(mockUserId);
      expect(gestureFindAllStub.calledOnce).to.be.true;
      expect(response).to.deep.equal([
        {
          gesture: `${mockGesture}-1`,
          phoneAction: `${mockPhoneAction}-1`,
          presetName: `${mockPreset.name}-1`,
        },
        {
          gesture: `${mockGesture}-2`,
          phoneAction: `${mockPhoneAction}-2`,
          presetName: `${mockPreset.name}-2`,
        },
      ]);
    });
  });

  describe('updateGestureSetting', () => {
    beforeEach(() => {
      presetFindOneStub.resolves({id: mockPreset.id});
    });

    it('updates the phone action', async () => {
      await service.updateGestureSetting(mockUserId, {
        gesture: mockGesture,
        presetName: mockPreset.name,
      }, {phoneAction: mockPhoneAction});
      expect(gestureUpsertStub.calledOnceWithExactly({
        gesture: mockGesture,
        phoneAction: mockPhoneAction,
        presetId: mockPreset.id,
      }, {
        fields: ['phoneAction'],
      })).to.be.true;
    });
  });
});
