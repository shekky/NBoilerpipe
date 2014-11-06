﻿using System;
using Shaman.Dom;

namespace NBoilerpipe.Parser
{
    public interface IContentHandler
    {
        void StartElement(HtmlNode node);
		void EndElement(HtmlNode node);
        void HandleText(HtmlTextNode node);
    }
}
