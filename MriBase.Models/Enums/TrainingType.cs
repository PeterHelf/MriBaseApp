using System;

namespace MriBase.Models.Enums
{
    [Serializable]
    public enum TrainingType
    {
        RndPosTest,
        TwoImgTest,
        SeveralImgTest,
        MatchingToSample,
        GoNoGo,
        SequentialLearning,
        DeathRecognitionTraining1,
        DeathRecognitionTraining2,
        DeathRecognition,
        SingleImageTraining,
        EntireTouchscreenTraining,
        SingleImageMultiplePositionsTraining,
    }
}