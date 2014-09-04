function getDHTMLObjHeight(_1){
return parseInt(_1.offsetHeight,10);
}
function getPixelsFromTop(_2){
var _3=(theBrowser.code=="MSIE")?_2.pixelTop:_2.top;
if(typeof (_3)=="string"){
_3=parseInt(_3);
}
return _3;
}
function simpleArray(){
this.item=0;
}
function imgStoreItem(n,s,w,h){
this.name=n;
this.src=s;
this.obj=null;
this.w=w;
this.h=h;
if((theBrowser.canCache)&&(s)){
this.obj=new Image(w,h);
this.obj.src=s;
}
}
function imgStoreObject(){
this.count=-1;
this.img=new imgStoreItem;
this.find=imgStoreFind;
this.add=imgStoreAdd;
this.getSrc=imgStoreGetSrc;
this.getTag=imgStoreGetTag;
}
function imgStoreFind(_8){
var _9=-1;
for(var i=0;i<=this.count;i++){
if(this.img[i].name==_8){
_9=i;
break;
}
}
return _9;
}
function imgStoreAdd(n,s,w,h){
var i=this.find(n);
if(i==-1){
i=++this.count;
}
this.img[i]=new imgStoreItem(n,s,parseInt(w,10),parseInt(h,10));
}
function imgStoreGetSrc(_10){
var i=this.find(_10);
var img=this.img[i];
return (i==-1)?"":img.src;
}
function imgStoreGetTag(_13,_14,_15){
var tag="";
var i=this.find(_13);
if(i<0){
return "";
}
with(this.img[i]){
if(src!=""){
tag="<img src=\""+src+"\"";
tag+=(_14!="")?" id=\""+_14+"\"":"";
tag+=" alt=\""+((_15)?_15:"")+"\" />";
}
}
return tag;
}
function TocItem(_18,_19,_1a,_1b,_1c,_1d,_1e,_1f,_20,_21){
var t=this;
this.owner=_18;
this.isLeaf=_1a;
this.type=(_1a==true)?"Document":"Folder";
this.text=_1b;
this.topicId=_1c;
this.parentTopicId=_1d;
this.depth=_21;
this.topicUrl="../"+_1c+"."+CFG_HTML_EXT;
this.thisIndex=_19;
this.prevEntry=null;
this.nextEntry=null;
this.parent=_20;
this.firstChild=null;
this.prevSibling=_1f;
this.nextSibling=_1e;
this.isopen=(_18==-1)?true:false;
this.isSelected=false;
this.isDrawn=false;
this.draw=MIDraw;
this.getDomNode=MIGetDomNode;
this.getInnerHTML=MIGetInnerHTML;
this.PMIconName=MIGetPMIconName;
this.docIconName=MIGetDocIconName;
this.setImg=MISetImage;
this.setIsOpen=MISetIsOpen;
this.setSelected=MISetSelected;
this.setIcon=MISetIcon;
this.mouseOver=MIMouseOver;
this.mouseOut=MIMouseOut;
var i=(this.owner.imgStore)?this.owner.imgStore.find(this.type):-2;
if(i==-1){
i=this.owner.imgStore.find("iconPlus");
}
this.height=(i>-1)?this.owner.imgStore.img[i].h:0;
}
function MIGetDomNode(){
return (this.isDrawn==true)?dom_getEl(document,"entryDIV"+this.thisIndex):null;
}
function MIGetInnerHTML(){
var o=this.owner;
var _25="=\"return "+o.name;
var tmp=_25+".entries["+this.thisIndex+"].";
var _27=" onmouseover"+tmp+"mouseOver('";
var _28=" onmouseout"+tmp+"mouseOut('";
var _29=o.imgStore.getTag(this.PMIconName(),"plusMinusIcon"+this.thisIndex,"");
var _2a="";
if(this.isLeaf==false){
_2a+="<a href=\"#\" onclick"+_25+".toggleExpand("+this.thisIndex+",event);\""+_27+"plusMinusIcon',this);\""+_28+"plusMinusIcon');\">"+_29+"</a>";
}else{
_2a+=_29;
}
var tip=this.text;
var _2c=o.imgStore.getTag(this.docIconName(),"docIcon"+this.thisIndex,tip);
var _2d="<span class=\""+((this.isLeaf==false)?"node":"leaf")+"\">";
var _2e="<a id=\"tocentry"+this.thisIndex+"\" ";
if(this.topicUrl!=""){
_2e+=" href=\""+this.topicUrl+"\" onclick"+_25+".itemClicked("+this.thisIndex+",event);\""+_27+"docIcon',this);\""+_28+"docIcon');\"";
}
_2e+=(tip)?" title=\""+tip+"\">":">";
_2a+=_2d+_2e+_2c+this.text+"</a></span>";
return _2a;
}
function MIGetPMIconName(){
var n="icon"+((this.isLeaf==false)?((this.isopen==true)?"Minus":"Plus"):"Join");
n+=(this==this.owner.firstEntry)?((this.nextSibling==null)?"Only":"Top"):((this.nextSibling==null)?"Bottom":"");
return n;
}
function MIGetDocIconName(){
var is=this.owner.imgStore;
var n=this.type;
n+=((this.isopen)&&(is.getSrc(n+"Expanded")!=""))?"Expanded":"";
n+=((this.isSelected)&&(is.getSrc(n+"Selected")!=""))?"Selected":"";
return n;
}
function MISetImage(_32,_33){
var o=this.owner;
var s=o.imgStore.getSrc(_33);
if((s!="")&&(theBrowser.canCache)&&(!o.isBusy)){
var img=dom_getEl(document,_32);
if(img&&img.src!=s){
img.src=s;
}
}
}
function MISetIsOpen(_37){
if((this.isopen!=_37)&&(this.isLeaf==false)){
this.isopen=_37;
this.setImg("plusMinusIcon"+this.thisIndex,this.PMIconName());
this.setImg("docIcon"+this.thisIndex,this.docIconName());
return true;
}else{
return false;
}
}
function MISetSelected(_38){
this.isSelected=_38;
var s=dom_getEl(document,"tocentry"+this.thisIndex);
if(s&&s.style){
if(_38){
s.style.color=(CFG_HELP_TYPE=="tablethtml")?"#a40000":"#cc0000";
s.style.fontWeight="bold";
}else{
s.style.color="#333333";
s.style.fontWeight="normal";
}
}
this.setImg("docIcon"+this.thisIndex,this.docIconName());
if((this.parent!=null)&&this.owner.selectParents){
this.parent.setSelected(_38);
}
}
function MISetIcon(_3a){
this.type=_3a;
this.setImg("docIcon"+this.thisIndex,this.docIconName());
}
function MIMouseOver(_3b,_3c){
var _3d="";
var s="";
if(_3b=="plusMinusIcon"){
_3d=this.PMIconName();
s="Click to "+((this.isopen==true)?"collapse.":"expand.");
}else{
if(_3b=="docIcon"){
_3d=this.docIconName();
}
}
if(theBrowser.canOnMouseOut){
this.setImg(_3b+this.thisIndex,_3d+"MouseOver");
}
if(this.onMouseOver){
var me=this;
eval(me.onMouseOver);
}
return true;
}
function MIMouseOut(_40){
var _41="";
if(_40=="plusMinusIcon"){
_41=this.PMIconName();
}else{
if(_40=="docIcon"){
_41=this.docIconName();
}
}
this.setImg(_40+this.thisIndex,_41);
if(this.onMouseOut){
var me=this;
eval(me.onMouseOut);
}
return true;
}
function Toc(){
this.count=-1;
this.size=0;
this.firstEntry=null;
this.autoScrolling=true;
this.toggleOnLink=false;
this.name="theToc";
this.container="leftnav";
this.contentFrame="topic";
this.selectParents=false;
this.lastPMClicked=-1;
this.selectedEntry=-1;
this.isBusy=true;
this.isVisible=true;
this.imgStore=new imgStoreObject;
this.entries=new Array();
this.getEmptyEntryIndex=TocGetEmptyEntryIndex;
this.addEntry=TocAddEntry;
this.addToc=TocAddEntry;
this.addChild=TocAddChild;
this.refresh=TocRefresh;
this.scrollTo=TocScrollTo;
this.contentDiv=null;
this.syncWithPage=TocSyncWithPage;
this.itemClicked=TocItemClicked;
this.selectEntry=TocSelectEntry;
this.setEntry=TocSetEntry;
this.findEntry=TocFindEntry;
this.toggleExpand=TocToggleExpand;
this.load=TocLoad;
this.addLevel=TocAddLevel;
initOutlineIcons(this.imgStore);
}
function TocGetEmptyEntryIndex(){
for(var i=0;i<=this.count;i++){
if(this.entries[i]==null){
break;
}
}
if(i>this.count){
this.count=i;
}
return i;
}
function TocAddEntry(_44,_45,_46,_47,_48,_49,_4a){
var _4b=this.getEmptyEntryIndex();
var _4c=new TocItem(this,_4b,_46,_47,_48,_49,null,_45,_44,_4a);
this.entries[_4b]=_4c;
if(_45!=null){
_45.nextSibling=_4c;
}
if(_44!=null){
if(_45==null){
_44.firstChild=_4c;
}
}else{
this.firstEntry=_4c;
}
_4c.prevEntry=(_45!=null)?_45:_44;
if(_4c.prevEntry!=null){
_4c.nextEntry=_4c.prevEntry.nextEntry;
_4c.prevEntry.nextEntry=_4c;
}
if(_4c.nextEntry!=null){
_4c.nextEntry.prevEntry=_4c;
}
_4c.draw(this.contentDiv);
this.size++;
return _4c;
}
function TocAddChild(_4d,_4e,_4f,_50,_51,_52){
var _53=null;
var _54=null;
if(_4d!=null){
_54=_4d.firstChild;
if(_54!=null){
while(_54.nextSibling!=null){
_54=_54.nextSibling;
}
}
}
return this.addEntry(_4d,_54,_4e,_4f,_50,_51,_52);
}
function TocSyncWithPage(_55){
if(!this.Busy){
if(_55){
var eID=this.findEntry(_55);
if(eID>=0){
this.selectEntry(eID);
this.setEntry(eID,true);
if(this.autoScrolling){
this.refresh();
this.scrollTo(eID);
}
}
}
}
}
function TocRefresh(){
if(this.Busy==true){
return;
}
var _57=17;
var _58=new simpleArray;
var _59=this.firstEntry;
var _5a=(_59==null)?0:1;
var _5b=true;
var _5c=1;
var _5d=dom_getEl(document,"tocTop");
var _5e=_5d?_5d.offsetHeight:0;
var _5f=(CFG_HELP_TYPE=="tablethtml")?9:0;
var _60=null;
while(_5a>0){
_60=_59.getDomNode();
if(_5b){
if(CFG_RTL_TEXT==true){
_60.style.right=(_59.depth*_57)+_5f+"px";
}else{
_60.style.left=(_59.depth*_57)+_5f+"px";
}
_60.style.top=_5e+"px";
_60.style.visibility="visible";
_5e+=getDHTMLObjHeight(_60);
_5c=_5a;
}else{
_60.style.visibility="hidden";
_60.style.top="0px";
}
if(_59.firstChild!=null){
_5b=(_59.isopen==true)&&_5b;
_58[_5a++]=_59.nextSibling;
_59=_59.firstChild;
}else{
if(_59.nextSibling!=null){
_59=_59.nextSibling;
}else{
while(_5a>0){
if(_58[--_5a]!=null){
_59=_58[_5a];
_5b=(_5c>=_5a);
break;
}
}
}
}
}
}
function MIDraw(_61){
var _62=document.createElement("div");
_62.id="entryDIV"+this.thisIndex;
_62.className="tocItem";
_62.innerHTML=this.getInnerHTML();
if(this.nextEntry!=null&&this.nextEntry.isDrawn==true){
_61.insertBefore(_62,this.nextEntry.getDomNode());
}else{
_61.appendChild(_62);
}
this.isDrawn=true;
return _62;
}
function TocScrollTo(_63){
var e=this.entries[_63];
if(!e){
return;
}
var _65=getPixelsFromTop(document.getElementById("entryDIV"+_63).style);
var _66=_65+18;
var _67=dom_getScrollY();
var _68=_67+dom_getClientHeight();
if((_66>_68-20)||(_65<_67+30)){
var _69=_66-_68+(_68-_67)/2;
if(_65<(_67+_69)){
_69=_65-_67-(_68-_67)/2;
}
window.scrollBy(0,_69);
}
}
function TocItemClicked(_6a,_6b,_6c){
var r=true;
var e=this.entries[_6a];
var b=theBrowser;
if(!_6c){
this.selectEntry(_6a);
}
if(e.onClickFunc){
e.onClick=e.onClickFunc;
}
if(e.onClick){
var me=e;
if(eval(me.onClick)==false){
r=false;
}
}
if(r){
if(this.toggleOnLink&&(e.isLeaf==false)&&!_6c){
this.toggleExpand(_6a,_6b,true);
}
}
return (e.topicUrl!="")?r:false;
}
function TocSelectEntry(_71){
var oe=this.entries[this.selectedEntry];
if(oe){
oe.setSelected(false);
}
var e=this.entries[_71];
if(e){
e.setSelected(true);
}
this.selectedEntry=_71;
}
function TocSetEntry(_74,_75){
var cl=","+_74+",";
var e=this.entries[_74];
this.lastPMClicked=_74;
var mc=e.setIsOpen(_75);
var p=e.parent;
while(p!=null){
cl+=p.thisIndex+",";
mc|=(p.setIsOpen(true));
p=p.parent;
}
return mc;
}
function TocFindEntry(_7a,_7b,_7c,_7d){
var e;
var sf;
if(_7a==""){
return -1;
}
if(!_7b){
_7b="topicId";
}
if(!_7c){
_7c="exact";
}
if(!_7d){
_7d=0;
}
if(_7b=="URL"||_7b=="url"){
_7b="topicUrl";
}
if(_7b=="title"){
_7b="text";
}
eval("sf = cmp_"+_7c);
for(var i=_7d;i<=this.count;i++){
if(this.entries[i]){
e=this.entries[i];
if(sf(eval("e."+_7b),_7a)){
return i;
}
}
}
return -1;
}
function cmp_exact(c,s){
return (c==s);
}
function cmp_left(c,s){
var l=Math.min(c.length,s.length);
return ((c.substring(1,l)==s.substring(1,l))&&(c!=""));
}
function cmp_right(c,s){
c=c.replace(/\.\./,"");
c=c.replace(/\\/g,"/");
s=s.replace(/\\/g,"/");
var l=Math.min(c.length,s.length);
return ((c.substring(c.length-l)==s.substring(s.length-l))&&(c!=""));
}
function cmp_contains(c,s){
return (c.indexOf(s)>=0);
}
function TocToggleExpand(_8b,e,_8d,_8e){
var e=e||event;
var _8f=e.shiftKey;
var _90=this.entries[_8b];
if(_90.isLeaf==false){
if(typeof (_8e)=="undefined"){
_8e=_90.isopen^1;
}
if(_90.firstChild==null&&_8e){
this.load(_90.topicId,_90.parentTopicId);
}
if(_8f&&_90.firstChild){
var _91=_90.firstChild;
while(_91){
this.toggleExpand(_91.thisIndex,e,_8d,_8e);
_91=_91.nextSibling;
}
}
var chg=this.setEntry(_8b,_8e);
if(chg){
this.refresh();
}
}
return false;
}
function TocAddLevel(_93,_94){
if(!_93||_93.firstChild==null){
var _95=null;
var _96=0;
var _97=_94.attributes.getNamedItem("id").nodeValue;
if(!_93){
var _98=_94.getElementsByTagName("title");
if(_98.length){
var _99=_98[0].childNodes[0].nodeValue;
_95=this.addChild(_95,false,_99,_97,null,_96);
}
}else{
_95=_93;
_96=_93.depth;
}
var _9a=_94.getElementsByTagName("entry");
for(var i=0;i<_9a.length;i++){
var _9c=_9a[i];
var _99=_9c.childNodes[0].nodeValue;
var _9d=(_9c.attributes.getNamedItem("leaf").nodeValue=="true")?true:false;
var _9e=_9c.attributes.getNamedItem("id").nodeValue;
this.addChild(_95,_9d,_99,_9e,_97,_96+1);
}
}else{
}
}
function TocLoad(_9f,_a0,_a1){
var _a2=true;
var _a3=false;
var _a4=null;
var _a5=(_9f!=null)?this.findEntry(_9f):-1;
if(_a5==-1){
if(_a0){
_a2=this.load(_a0,null,_a1);
if(_a2==true){
_a5=this.findEntry(_9f);
_a4=this.entries[_a5];
_a3=_a4.isLeaf==false?true:false;
}
}else{
_a3=true;
}
}else{
_a4=this.entries[_a5];
if(_a4.isLeaf==false&&_a4.firstChild==null){
_a3=true;
}
}
if(_a3==true){
var _a6=dom_LoadXMLDoc("toc_"+_9f+".xml");
if(_a6==null){
_a6=dom_LoadXMLDoc("ui/toc_"+_9f+".xml");
}
if(_a6){
var _a7=_a6.getElementsByTagName("tocsection");
if(_a7.length==1){
var _a8=_a7[0];
var _a9=_a8.attributes.getNamedItem("parent");
var _aa=null;
if(_a9&&_a9.nodeValue.length){
var _ab=this.findEntry(_a9.nodeValue);
_aa=(_ab!=-1)?this.entries[_ab]:null;
}
if(_a9&&_a9.nodeValue.length&&(_aa==null||_aa.firstChild==null)){
_a2=this.load(_a9.nodeValue,null,_a1);
if(_a2==true){
_a5=this.findEntry(_9f);
_a4=this.entries[_a5];
}
}
this.addLevel(_a4,_a8);
_a2=true;
}else{
_a2=false;
}
}else{
_a2=false;
}
}
return _a2;
}
function browserInfo(){
this.code="unknown";
this.version=0;
this.platform="Win";
var ua=navigator.userAgent;
var i=ua.indexOf("WebTV");
if(i>=0){
this.code="WebTV";
i+=6;
}else{
i=ua.indexOf("Opera");
if(i>=0){
this.code="OP";
i=ua.indexOf(") ")+2;
}else{
i=ua.indexOf("MSIE");
if(i>=0){
this.code="MSIE";
i+=5;
}else{
i=ua.indexOf("Mozilla/");
if(i>=0){
this.code="NS";
i+=8;
}
}
}
}
this.version=parseFloat(ua.substring(i,i+4));
if(ua.indexOf("Mac")>=0){
this.platform="Mac";
}
if(ua.indexOf("OS/2")>=0){
this.platform="OS/2";
}
if(ua.indexOf("X11")>=0){
this.platform="UNIX";
}
var xx=navigator.appName;
var v=this.version;
var p=this.platform;
var NS=(this.code=="NS");
var IE=(this.code=="MSIE");
var WTV=(this.code=="WebTV");
var OP=(this.code=="OP");
var _b5=(OP&&(v>=3.2));
var _b6=(OP&&(v>=5));
var _b7=(IE&&(v>=4));
var _b8=(NS&&(v>=3));
var _b9=(NS&&(v>=5));
this.NWCode=((xx.indexOf("Netware")>=0)||(ua.indexOf("ICEBrowser")>=0));
this.canCache=_b8||_b7||_b5||WTV;
this.canOnMouseOut=this.canCache;
this.canOnError=_b8||_b7||_b5;
this.canJSVoid=!((NS&&!_b8)||(IE&&!_b7)||(OP&&(v<3.5)));
this.hasDHTML=((NS||IE)&&(v>=4))&&!(IE&&(p=="Mac")&&(v<4.5))||this.NWCode;
this.hasW3CDOM=(document.getElementById)?true:false;
this.DHTMLRange=IE?".all":"";
this.DHTMLStyleObj=IE?".style":"";
this.DHTMLDivHeight=IE?".offsetHeight":".clip.height";
}
function defOnError(msg,_bb,lno){
if(jsErrorMsg==""){
return false;
}else{
alert(jsErrorMsg+".\n\nError: "+msg+"\nPage: "+_bb+"\nLine: "+lno+"\nBrowser: "+navigator.userAgent);
return true;
}
}
function initOutlineIcons(_bd){
var _be=18;
var ip=CFG_BOOKUI2SHAREDUI_RELPATH+"images/";
var _c0=(CFG_HELP_TYPE=="tablethtml")?"t":"";
_bd.add("iconPlusTop",ip+"plustop.png",_be,_be);
_bd.add("iconPlus",ip+_c0+"plus.png",_be,_be);
_bd.add("iconPlusBottom",ip+_c0+"plusbottom.png",_be,_be);
_bd.add("iconPlusOnly",ip+"plusonly.png",_be,_be);
_bd.add("iconMinusTop",ip+"minustop.png",_be,_be);
_bd.add("iconMinus",ip+_c0+"minus.png",_be,_be);
_bd.add("iconMinusBottom",ip+_c0+"minusbottom.png",_be,_be);
_bd.add("iconMinusOnly",ip+_c0+"minusonly.png",_be,_be);
_bd.add("iconLine",ip+"line.png",_be,_be);
_bd.add("iconBlank",ip+"blank.png",_be,_be);
_bd.add("iconJoinTop",ip+"jointop.png",_be,_be);
_bd.add("iconJoin",ip+"join.png",_be,_be);
_bd.add("iconJoinBottom",ip+"joinbottom.png",_be,_be);
_bd.add("Folder",ip+_c0+"folder.png",_be,_be);
_bd.add("FolderMouseOver",ip+_c0+"folder_mo.png",_be,_be);
_bd.add("FolderExpanded",ip+_c0+"folder_ex.png",_be,_be);
_bd.add("FolderExpandedMouseOver",ip+_c0+"folder_ex_mo.png",_be,_be);
_bd.add("FolderExpandedSelected",ip+_c0+"folder_ex_sel.png",_be,_be);
_bd.add("Document",ip+_c0+"doc.png",_be,_be);
_bd.add("DocumentMouseOver",ip+_c0+"doc_mo.png",_be,_be);
_bd.add("DocumentSelected",ip+_c0+"doc_sel.png",_be,_be);
}
function TocUpdate(_c1){
var _c2=false;
var _c3=null;
var _c4=null;
var _c5=top.frames[theToc.contentFrame];
if(_c5&&typeof (_c5.thisId)!="undefined"){
_c3=_c5.thisId;
_c4=_c5.parentId;
}else{
if(typeof (CFG_MAIN_TOPIC)!="undefined"){
_c3=CFG_MAIN_TOPIC.replace("."+CFG_HTML_EXT,"");
}
}
_c2=theToc.load(_c3,_c4);
if(_c2==true){
theToc.syncWithPage(_c3);
}else{
if(typeof _c1=="undefined"){
dom_getEl(document,"tocContent").innerHTML="<div class='entry0'>Contents not available.</div>"+"<div class='entry0'>Unable to load xml content.</div>";
}
}
return _c2;
}
function TocUpdateFromTopic(){
if(!theToc){
setTimeout("TocUpdateFromTopic()",200);
}
if(theToc.isVisible){
if(!theToc.isBusy){
if(TocUpdate(false)==false){
setTimeout("TocUpdate()",0);
}
}else{
setTimeout("TocUpdateFromTopic()",200);
}
}
}
function initToc(){
theToc.contentDiv=dom_getEl(document,"tocContent");
theToc.isBusy=false;
}
var theBrowser=new browserInfo;
var jsErrorMsg="A JavaScript error has occurred:";
if(theBrowser.canOnError){
self.onerror=defOnError;
}
var theToc=new Toc;

