'use strict';

const sinon = require('sinon');
const proxyquire = require('proxyquire').noCallThru();
const expect = require('chai').expect;

describe('services -> featureRequests', () => {
  let service;
  let sandbox;

  let featureRequestCreateStub;
  let featureRequestDestroyStub;
  let featureRequestFindAllStub;
  let featureRequestFindOneStub;
  const featureRequestMock = {
    create: (...values) => featureRequestCreateStub(...values),
    destroy: (...values) => featureRequestDestroyStub(...values),
    findAll: (...values) => featureRequestFindAllStub(...values),
    findOne: (...values) => featureRequestFindOneStub(...values),
  };

  let featureRequestVoteCreateStub;
  let featureRequestVoteDestroyStub;
  const featureRequestVoteMock = {
    create: (...values) => featureRequestVoteCreateStub(...values),
    destroy: (...values) => featureRequestVoteDestroyStub(...values),
  };

  const mockDescription = 'mockDescription';
  const mockFeatureRequestId = 'mockFeatureRequestId';
  const mockStatus = 'mockStatus';
  const mockTags = ['mockTagOne', 'mockTagTwo'];
  const mockUserId = 'userId';
  
  before(() => {
    sandbox = sinon.createSandbox();

    featureRequestCreateStub = sandbox.stub();
    featureRequestDestroyStub = sandbox.stub();
    featureRequestFindAllStub = sandbox.stub();
    featureRequestFindOneStub = sandbox.stub();

    featureRequestVoteCreateStub = sandbox.stub();
    featureRequestVoteDestroyStub = sandbox.stub();

    service = proxyquire('../../../src/services/featureRequests', {
      '../../models': {
        sequelize: {
          FeatureRequest: featureRequestMock,
          FeatureRequestVote: featureRequestVoteMock,
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

  describe('addFeatureRequestVote', () => {
    beforeEach(() => {
      featureRequestFindOneStub
        .withArgs({where: {id: mockFeatureRequestId}})
        .resolves({id: mockFeatureRequestId});
      featureRequestVoteCreateStub.resolves();
    });

    it('creates a feature request vote', async () => {
      await service.addFeatureRequestVote(mockUserId, {
        featureRequestId: mockFeatureRequestId,
      });
      expect(featureRequestFindOneStub.calledOnce).to.be.true;
      expect(featureRequestVoteCreateStub.calledOnceWith({
        featureRequestId: mockFeatureRequestId,
        userId: mockUserId,
      })).to.be.true;
    });
  });

  describe('createFeatureRequest', () => {
    beforeEach(() => {
      featureRequestCreateStub.resolves();
    });

    it('creates a feature request', async () => {
      await service.createFeatureRequest(mockUserId, {
        description: mockDescription,
        tags: mockTags,
      });
      expect(featureRequestCreateStub.calledOnceWith({
        description: mockDescription,
        requestorId: mockUserId,
        tags: mockTags.join(','),
      })).to.be.true;
    });
  });

  describe('deleteFeatureRequest', () => {
    beforeEach(() => {
      featureRequestDestroyStub.resolves();
    });

    it('deletes the feature request', async () => {
      await service.deleteFeatureRequest({
        featureRequestId: mockFeatureRequestId,
      });
      expect(featureRequestDestroyStub.calledOnceWith({
        where: {
          id: mockFeatureRequestId,
        },
      }));
    });
  });

  describe('getFeatureRequests', () => {
    const getFeatureRequest = (number) => {
      return {
        createdAt: `createdAt${number}`,
        description: `description${number}`,
        id: number,
        requestorId: `requestorId${number}`,
        status: `status${number}`,
        tags: `tag${number},extraTag`,
        FeatureRequestVotes: [
          {userId: mockUserId},
          {userId: 'other'},
          {userId: 'userId'},
        ],
      };
    };
    const featureRequests = [
      getFeatureRequest(0),
      getFeatureRequest(1),
    ];

    beforeEach(() => {
      featureRequestFindAllStub.resolves(featureRequests);
    });

    it('formats result properly', async () => {
      const result = await service.getFeatureRequests(mockUserId);
      expect(result.length).to.equal(2);
      expect(result).to.deep.equal([
        {
          createdAt: 'createdAt0',
          description: 'description0',
          id: 0,
          requestorId: 'requestorId0',
          status: 'status0',
          tags: ['tag0', 'extraTag'],
          votes: 3,
          userVotedForThisFeature: true,
        },
        {
          createdAt: 'createdAt1',
          description: 'description1',
          id: 1,
          requestorId: 'requestorId1',
          status: 'status1',
          tags: ['tag1', 'extraTag'],
          votes: 3,
          userVotedForThisFeature: true,
        }
      ]);
    });

    it('treats status as optional', async () => {
      const result = await service.getFeatureRequests(mockUserId);
      const options = {
        include: featureRequestVoteMock,
        where: undefined,
      };
      expect(featureRequestFindAllStub.calledOnceWith(options)).to.be.true;
      expect(result.length).to.equal(2);
    });

    it('filters based on provided status', async () => {
      const result = await service.getFeatureRequests(mockUserId, mockStatus);
      const options = {
        include: featureRequestVoteMock,
        where: {status: mockStatus},
      };
      expect(featureRequestFindAllStub.calledOnceWith(options)).to.be.true;
      expect(result.length).to.equal(2);
    });
  });

  describe('removeFeatureRequestVote', async () => {
    beforeEach(() => {
      featureRequestVoteDestroyStub.resolves();
    });

    it('removes the feature request vote', async () => {
      await service.removeFeatureRequestVote(mockUserId, {
        featureRequestId: mockFeatureRequestId,
      });
      expect(featureRequestVoteDestroyStub.calledOnceWith({
        where: {
          featureRequestId: mockFeatureRequestId,
          userId: mockUserId,
        },
      })).to.be.true;
    });
  });

  describe('updateFeatureRequestStatus', async () => {
    let saveStub;

    beforeEach(() => {
      saveStub = sandbox.stub();
    });

    it('updates the feature request if it exists', async () => {
      featureRequestFindOneStub
        .withArgs({where: {id: mockFeatureRequestId}})
        .resolves({
          save: (...values) => saveStub(...values),
        });
      await service.updateFeatureRequestStatus({
        featureRequestId: mockFeatureRequestId,
        status: mockStatus,
      });
      expect(saveStub.calledOnce).to.be.true;
    });

    it('ignores the request if the feature request doesn\'t exist', async () => {
      featureRequestFindOneStub
        .withArgs({where: {id: mockFeatureRequestId}})
        .resolves(undefined);
      await service.updateFeatureRequestStatus({
        featureRequestId: mockFeatureRequestId,
        status: mockStatus,
      });
      expect(saveStub.calledOnce).to.be.false;
    });
  }); 
});
