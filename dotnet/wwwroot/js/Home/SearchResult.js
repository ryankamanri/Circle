import SearchUserInfo from '../Shared/SearchUserInfo.js'
import Post from '../Shared/Post.js'
import MainVue from '../MainVue.js'


let userContainer,searchResult,searchString,splitedSearchStrings;
let vue;
function SearchResult()
{
    vue = MainVue.MainVue();

    ///由于这一部分作用为替换关键字为高亮,将部分dom元素进行了替换,所以在此之前绑定过的监听元素会失效
    ///如需绑定监听元素,请在此段之后
    searchResult = document.querySelector("#search-result").innerHTML;
    searchString = document.querySelector("input#search").value;
    splitedSearchStrings = searchString.split(" ");
    
    let divMatchStrings = searchResult.match(/<div.*>\s*.*\s*<\/div>/g);
    if(divMatchStrings != undefined) divMatchStrings.forEach(matchString => 
        splitedSearchStrings.forEach(splitedSearchString => HignlightReplace(matchString,splitedSearchString)));
    let spanMatchStrings = searchResult.match(/<span.*>\s*.*\s*<\/span>/g);
    if(spanMatchStrings != undefined) spanMatchStrings.forEach(matchString => 
        splitedSearchStrings.forEach(splitedSearchString => HignlightReplace(matchString,splitedSearchString)));
    document.querySelector("#search-result").innerHTML = searchResult;
    ///

    SearchUserInfo.SearchUserInfo(vue);
    Post.Post(vue);

}

function HignlightReplace(matchString,searchString)
{
    let matchAttr = matchString.match(/>\s*.*\s*</g)[0];
    let matchAttrReplace = matchAttr.replaceAll(searchString,`<strong style="color: red">${searchString}</strong>`);
    let matchStringReplace = matchString.replace(matchAttr,matchAttrReplace);
    searchResult = searchResult.replace(matchString,matchStringReplace);
}

export default{
    SearchResult
}






