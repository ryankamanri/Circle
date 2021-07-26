import SearchUserInfo from '../Shared/SearchUserInfo.js'

let userContainer,searchResult,searchString;
function SearchResult()
{
    
    ///由于这一部分作用为替换关键字为高亮,将部分dom元素进行了替换,所以在此之前绑定过的监听元素会失效
    ///如需绑定监听元素,请在此段之后
    searchResult = document.querySelector("#search-result").innerHTML;
    searchString = document.querySelector("input#search").value;
    
    let divMatchStrings = searchResult.match(/<div.*>.*<\/div>/g);
    if(divMatchStrings != undefined) divMatchStrings.forEach(matchString => HignlightReplace(matchString));
    let spanMatchStrings = searchResult.match(/<span.*>.*<\/span>/g);
    if(spanMatchStrings != undefined) spanMatchStrings.forEach(matchString => HignlightReplace(matchString));
    document.querySelector("#search-result").innerHTML = searchResult;
    ///

    SearchUserInfo.SearchUserInfo();

}

function HignlightReplace(matchString)
{
    let matchAttr = matchString.match(/>.*</g)[0];
    let matchAttrReplace = matchAttr.replaceAll(searchString,`<strong style="color: red">${searchString}</strong>`);
    let matchStringReplace = matchString.replace(matchAttr,matchAttrReplace);
    searchResult = searchResult.replace(matchString,matchStringReplace);
}

export default{
    SearchResult
}






