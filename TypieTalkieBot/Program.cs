// ------------------------------------
// Typie Talkie AI Bot
// ------------------------------------

using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.Extensions.Configuration;
using TypieTalkieBot;

var builder = new ConfigurationBuilder()
    .AddUserSecrets<Program>();

var config = builder.Build();

var speechKey = config["SpeechKey"];
var speechRegion = config["SpeechRegion"];

var speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);
speechConfig.SpeechRecognitionLanguage = "en-US";
speechConfig.SpeechSynthesisVoiceName = "en-US-AvaMultilingualNeural";

await Speak(speechConfig);
await TranscribeDictation(speechConfig);

async static Task Speak(SpeechConfig speechConfig)
{
    var textToSpeechProcessor = new TextToSpeechProcessor();

    using var speechSynthesizer = new SpeechSynthesizer(speechConfig);

    Console.WriteLine("Enter some text that you want to speak >");
    string? text = Console.ReadLine();
    text ??= "Why you no enter words?";

    var speechSynthesisResult = await speechSynthesizer.SpeakTextAsync(text);
    textToSpeechProcessor.OutputSpeechSynthesisResult(speechSynthesisResult, text);

    Console.WriteLine("Press any key to exit...");
    Console.ReadKey();
}

async static Task TranscribeDictation(SpeechConfig speechConfig)
{
    var speechToTextProcessor = new SpeechToTextProcessor();

    using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
    using var speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);

    Console.WriteLine("Press any key to start listening.");
    Console.ReadKey();
    Console.WriteLine("Speak into your microphone.");
    var speechRecognitionResult = await speechRecognizer.RecognizeOnceAsync();
    speechToTextProcessor.OutputSpeechRecognitionResult(speechRecognitionResult);
}