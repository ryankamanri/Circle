let tagComponent = {
	template : `
	<div class="tag" :id="tag.ID" draggable="true" :style="{background : bColor}">
		<div class="ID">{{tag.tagID}}</div>
		<div class="_Tag" :style="{color : fColor}">{{tag._Tag}}</div>
	</div>`,
	
	props : ['tag'],
	
	data() {
		return{
			bColor : this.tag.bColor,
			fColor : this.tag.fColor
		} 
	}
}