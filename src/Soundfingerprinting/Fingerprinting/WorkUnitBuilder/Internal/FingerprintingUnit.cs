namespace Soundfingerprinting.Fingerprinting.WorkUnitBuilder.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Soundfingerprinting.Audio.Services;
    using Soundfingerprinting.Fingerprinting.Configuration;

    internal sealed class FingerprintingUnit : ITargetOn, IWithConfiguration, IFingerprintingUnit
    {
        private readonly IAudioService audioService;
        private readonly IFingerprintService fingerprintService;

        private Func<Task<List<bool[]>>> createFingerprintsMethod;

        public FingerprintingUnit(IFingerprintService fingerprintService, IAudioService audioService)
        {
            this.fingerprintService = fingerprintService;
            this.audioService = audioService;
        }

        public IFingerprintingConfiguration Configuration { get; private set; }

        public Task<List<bool[]>> RunAlgorithm()
        {
            return createFingerprintsMethod();
        }

        public IWithConfiguration On(string pathToAudioFile)
        {
            createFingerprintsMethod = () =>
                {
                    float[] samples = audioService.ReadMonoFromFile(pathToAudioFile, Configuration.SampleRate, 0, 0);
                    return fingerprintService.CreateFingerprints(samples, Configuration);
                };

            return this;
        }

        public IWithConfiguration On(float[] audioSamples)
        {
            createFingerprintsMethod = () => fingerprintService.CreateFingerprints(audioSamples, Configuration);
            return this;
        }

        public IWithConfiguration On(string pathToAudioFile, int millisecondsToProcess, int startAtMillisecond)
        {
            createFingerprintsMethod = () =>
                {
                    float[] samples = audioService.ReadMonoFromFile(pathToAudioFile, Configuration.SampleRate, millisecondsToProcess, startAtMillisecond);
                    return fingerprintService.CreateFingerprints(samples, Configuration);
                };
            return this;
        }

        public IFingerprintingUnit With(IFingerprintingConfiguration configuration)
        {
            Configuration = configuration;
            return this;
        }

        public IFingerprintingUnit With<T>() where T : IFingerprintingConfiguration, new()
        {
            Configuration = new T();
            return this;
        }

        public IFingerprintingUnit WithCustomConfiguration(Action<CustomFingerprintingConfiguration> transformation)
        {
            CustomFingerprintingConfiguration customFingerprintingConfiguration = new CustomFingerprintingConfiguration();
            Configuration = customFingerprintingConfiguration;
            transformation(customFingerprintingConfiguration);
            return this;
        }
    }
}