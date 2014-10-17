'use strict'
/*
    nodes:
    [
      {
        id,
        poeng,
        navn,
        (radius blir satt)
      }
    ]
  */
  var BubbleScoreboard = function(nodes) {
    var self = this;
    this.nodes = nodes;
    this.circles = null;
    this.vis = null;
    this.width = 800;
    this.height = 600;
    this.center = {x: this.width / 2, y: this.height / 2}

    this.min_scale = 50;
    this.max_scale = 100;
    
    this.max_poeng = 0;

    //div ting fra eksempel
    this.layout_gravity = -0.01;
    this.damper = 0.02; //gravitasjon-avstand
    this.force = null;
    this.fill_color = d3.scale.category20()
      .domain(_.map(self.nodes, function(n) { return n.navn; }));

    this.setRadiusScale();

    this.updateNodeRadius();

    this.createSvg();
  }

  BubbleScoreboard.prototype.setRadiusScale = function() {
    this.max_poeng = d3.max(this.nodes, function (n) {
      return n.poeng;
    });

    this.radius_scale = d3.scale
      .pow()
      .exponent(0.5)
      .domain([0, this.max_poeng])
      .range([this.min_scale, this.max_scale]);
  }
  BubbleScoreboard.prototype.updateNodeRadius = function() {
    var self = this;
    this.nodes.forEach(function (d) {
      var poeng = d.poeng > 0 ? d.poeng : 0;

      d.radius = self.radius_scale(poeng);
    });
  }

  BubbleScoreboard.prototype.createSvg = function() {
    var self = this;

    //create cvs
    this.vis = d3.select("#scoreboard").append("svg")
      .attr("width", this.width)
      .attr("height", this.height)
      .attr("id", "svg_vis");

    this.groups = this.vis.selectAll("g")
      .data(this.nodes)
      .enter()
      .append("g");

    this.circles = this.groups.append("circle")
      .attr({
        "r": 0,
        "fill": function(d) { return self.fill_color(d.navn); },
        "stroke": function(d) { 
            //return d3.rgb(self.fill_color('#FF6400')).darker();
            return d3.rgb(255,100,0);
        },
        "stroke-width": 4,
        "id": function(d) { 
          return "bubble_" +d.id; 
        }
      });

    this.labels = this.groups.append("text")
      .attr({
        "alignment-baseline": "middle",
        "text-anchor": "middle",
        "font-size":"15px"
      })
      .text(function(d) { 
        return d.navn + " (" + d.poeng + ")";
      });

    // radius will be set to 0 initially.
    // see transition below
    this.circles
      

    //Fancy transition to make bubbles appear, ending with the
    //correct radius
    this.circles.transition().duration(2000).attr("r", function(d) { return d.radius; })
    
  };

  BubbleScoreboard.prototype.charge = function(d) {
    return -1 * Math.pow(d.radius, 2.0) / 8;
  };

  BubbleScoreboard.prototype.start = function() {
    return this.force = d3.layout.force()
      .nodes(this.nodes)
      .size([this.width, this.height]);
  };

  //Sets up force layout to display
  //all nodes in one circle.
  BubbleScoreboard.prototype.gravitate = function() {
    var self = this;

    this.force.gravity(this.layout_gravity)
      .charge(this.charge)
      .friction(0.9)
      .on("tick", function(e) {
        self.labels.each(self.move_towards_center(e.alpha))
          .attr("dx", function(d) { return d.x; })
          .attr("dy", function(d) { return d.y; })
          .text(function(d) { 
            return d.navn + "(" + d.poeng + ")";
          })
          ;

        self.circles.each(self.move_towards_center(e.alpha))
          .attr("cx", function(d) { return d.x; })
          .attr("cy", function(d) { return d.y; });
      });

    this.force.start()
  }

  //Moves all circles towards the @center
  //of the visualization
  BubbleScoreboard.prototype.move_towards_center = function(alpha) {
    var self = this;
    
    //curry!
    return function(d) {
      d.x = d.x + (self.center.x - d.x) * (self.damper + 0.02) * alpha
      d.y = d.y + (self.center.y - d.y) * (self.damper + 0.02) * alpha
    }
  }

  BubbleScoreboard.prototype.updateScore = function(id, score) {
    var node = _.where(this.nodes, {id: id})[0];
    node.poeng += score;

    this.setRadiusScale();
    this.updateNodeRadius();

    this.circles.transition().duration(100).attr("r", function(d) { return d.radius; });
    this.gravitate();
  };

  