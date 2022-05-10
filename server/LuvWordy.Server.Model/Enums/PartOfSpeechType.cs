using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuvWordy.Server.Model.Enums
{
    public enum PartOfSpeechType
    {
        // ?
        Unknown,
        // 명사
        Noun,
        // 동사
        Verb,
        // 대명사
        Pronoun,
        // 형용사
        Adjective,
        // 의존 명사
        DependentNoun,
        // 보조 동사
        AuxiliaryVerb,
        // 수사
        Numerals,
        // 조사
        Postposition,
        // 접사
        Affix,
        // 부사
        Adverb,
        // 감탄사
        Interjection,
        // 관형사
        Detective,
        // 보조 형용사
        AuxiliaryAdjective,
        // 어미
        Termination,
        // 품사 없음
        None,
    }
}
