window.net = window.net ||{ };
window.net.cartodb = new function (options) {
    that = this;

    function sendToDotNet(data) {
        if (typeof data === "object")
            data = J.stringify(data);

        console.log("[Net.Data:"+data+"]");
    }

	this.create = function (options) {
	    window.net.cartodb.map = new L.Map('cartodb-map', options);
	};

	this.plugin = function (data) {
	    data = data || { scripts:[], styles:[], initialize: null};
	    // data => { scripts:["","",..], styles:["","",..], initialize: function }

        //Embebed all css and js
	    // for (var i = 0; i < data.styles.length; i++)
	    // for (var i = 0; i < data.scripts.length; i++)

        //oncomplete --> if (typeof(data.initialize) === "function") data.initialize();
	}

	this.response = function(value) {
		this.value = value;
		this.errors = [];
		
		this.toString = function() {
		    return JSON.stringify(this);
		}
	};
	
	this.sumZoom = function (n) {
	    var n = window.net.cartodb.map.getZoom() + n;
	    window.net.cartodb.map.setZoom(n)

	    return new that.response([n]) + '';
	}

	this.setZoom = function(n) {
	    n = n || window.net.cartodb.map.getZoom() + 1;
	    window.net.cartodb.map.setZoom(n)
		
		return new that.response([n])+'';
	};
	
	this.getZoom = function() {
	    var n = window.net.cartodb.map.getZoom();
		
		return new that.response([n])+'';
	};

	this.OnMapEventFire = function (name, e) {
	    sendToDotNet({ name:name, e: e });
	}
}