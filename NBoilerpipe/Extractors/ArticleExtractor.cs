/*
 * This code is derived from boilerpipe
 * 
 */

using NBoilerpipe.Document;
using NBoilerpipe.Extractors;
using NBoilerpipe.Filters.English;
using NBoilerpipe.Filters.Heuristics;
using NBoilerpipe.Filters.Simple;
using Sharpen;

namespace NBoilerpipe.Extractors
{
    /// <summary>A full-text extractor which is tuned towards news articles.</summary>
    /// <remarks>
    /// A full-text extractor which is tuned towards news articles. In this scenario
    /// it achieves higher accuracy than
    /// <see cref="DefaultExtractor">DefaultExtractor</see>
    /// .
    /// </remarks>
    /// <author>Christian Kohlsch√ºtter</author>
    public sealed class ArticleExtractor : ExtractorBase
    {
        public static readonly ArticleExtractor INSTANCE = new ArticleExtractor();

        /// <summary>
        /// Returns the singleton instance for
        /// <see cref="ArticleExtractor">ArticleExtractor</see>
        /// .
        /// </summary>
        public static ArticleExtractor GetInstance()
        {
            return INSTANCE;
        }

        /// <exception cref="NBoilerpipe.BoilerpipeProcessingException"></exception>
        public override bool Process(TextDocument doc)
        {
            var ret = TerminatingBlocksFinder.INSTANCE.Process(doc)
                | new DocumentTitleMatchClassifier(doc.GetTitle()).Process(doc)
                | NumWordsRulesClassifier.INSTANCE.Process(doc)
                | IgnoreBlocksAfterContentFilter.DEFAULT_INSTANCE.Process(doc)
                | TrailingHeadlineToBoilerplateFilter.INSTANCE.Process(doc)
                | BlockProximityFusion.MAX_DISTANCE_1_NEAR_TAGLEVEL.Process(doc)
                | BoilerplateBlockFilter.INSTANCE_KEEP_TITLE.Process(doc)
                | BlockProximityFusion.MAX_DISTANCE_1_CONTENT_ONLY_SAME_TAGLEVEL.Process(doc)
                | KeepLargestBlockFilter.INSTANCE_EXPAND_TO_SAME_TAGLEVEL_MIN_WORDS.Process(doc)
                | ExpandTitleToContentFilter.INSTANCE.Process(doc)
                | LargeBlockSameTagLevelToContentFilter.INSTANCE.Process(doc)
                | ListAtEndFilter.INSTANCE.Process(doc);
            return ret;
        }
    }
}