namespace Boids

//TODO:  Create arraylist like object
//USING: https://processing.org/examples/flocking.html

(*
//CODE will be in unity calling
Flock flock;

void setup() {
  size(640, 360);
  flock = new Flock();
  // Add an initial set of boids into the system
  for (int i = 0; i < 150; i++) {
    flock.addBoid(new Boid(width/2,height/2));
  }
}

void draw() {
  background(50);
  flock.run();
}

// Add a new boid into the System
void mousePressed() {
  flock.addBoid(new Boid(mouseX,mouseY));
}

*)

type Vector(x : float, y : float) =
    member  this.X = x
    member  this.Y = y

    member this.add(v : Vector) =
         Vector(x + v.X, y + v.Y)
    
    member this.sub(v : Vector) =
         Vector(x - v.X, y - v.Y)

    member this.sub(first : Vector, second : Vector) =
         Vector(first.X - second.X, first.Y - second.Y)

    member this.mult(s : int) = 
         Vector(x * float s, y * float s)

    member this.mult(s : double) = 
         Vector(x * s, y * s)
    
    //Need to check to see if this is possible
    member this.limit(s : int) = 
        if x > float s then 
            this.X <- float s
        if y > float s then this.Y = float s



// The Flock (a list of Boid objects)

type Flock () = 
  member this.Boids = ArrayList<Boid> boids; // An ArrayList for all the boids

  member this.run() =
      for Boid b : Boids
          b.run(boids);  // Passing the entire list of boids to each boid individually

  member this.addBoid(b : boid) {
      Boids.add(b);

// The Boid class

type Boid (x : float, y: float) =

    member this.location = Vector(x , y)
    member this.r = 2.0
    member this.maxspeed = 2
    member this.maxforce = 0.03 
    member this.velocity = Vector(cos(random(3.14)), sin(random(3.14)))
    member this.acceleration = Vector(0 , 0)

    member this.run(boids : ArrayList<Boid> ) =
        this.flock(boids)
        this.update
        (*
        //call to run in unity?
        borders();
        render();
        *)
  

    member this.applyForce(force : Vector) =
        // We could add mass here if we want A = F / M
        this.acceleration.add(force);

      // We accumulate a new acceleration each time based on three rules
    member this.flock(ArrayList<Boid> boids) =
        let sep = separate(boids)   // Separation
        let ali = align(boids)     // Alignment
        let coh = cohesion(boids)   // Cohesion
        // Arbitrarily weight these forces
        sep.mult(1.5);
        ali.mult(1.0);
        coh.mult(1.0);
        // Add the force vectors to acceleration
        this.applyForce(sep);
        this.applyForce(ali);
        this.applyForce(coh);

      // Method to update location
    member this.update() =
        // Update velocity
        this.velocity.add(this.acceleration);
        // Limit speed
        this.velocity.limit(this.maxspeed);
        this.location.add(this.velocity);
        // Reset accelertion to 0 each cycle
        this.acceleration.mult(0);


    // A method that calculates and applies a steering force towards a target
    // STEER = DESIRED MINUS VELOCITY
    member this.seek(target : Vector) =
        let desired = Vector.sub(target, location);  // A vector pointing from the location to the target
        // Scale to maximum speed
        desired.normalize();
        desired.mult(maxspeed);

        // Above two lines of code below could be condensed with new PVector setMag() method
        // Not using this method until Processing.js catches up
        // desired.setMag(maxspeed);

        // Steering = Desired minus Velocity
        let steer = Vector.sub(desired, velocity);
        steer.limit(maxforce);  // Limit to maximum steering force

//  void render() {
//    // Draw a triangle rotated in the direction of velocity
//    float theta = velocity.heading2D() + radians(90);
//    // heading2D() above is now heading() but leaving old syntax until Processing.js catches up
//    
//    fill(200, 100);
//    stroke(255);
//    pushMatrix();
//    translate(location.x, location.y);
//    rotate(theta);
//    beginShape(TRIANGLES);
//    vertex(0, -r*2);
//    vertex(-r, r*2);
//    vertex(r, r*2);
//    endShape();
//    popMatrix();
//  }
//
//  // Wraparound
//  void borders() {
//    if (location.x < -r) location.x = width+r;
//    if (location.y < -r) location.y = height+r;
//    if (location.x > width+r) location.x = -r;
//    if (location.y > height+r) location.y = -r;
//  }

  // Separation
  // Method checks for nearby boids and steers away
    member this.separate (ArrayList<Boid> boids)  =
        let desiredseparation = 25.0f;
        let steer = new Vector(0, 0, 0);
        let count = 0;
        // For every boid in the system, check if it's too close
        for (Boid other : boids) 
            let d = Vector.dist(location, other.location);
            // If the distance is greater than 0 and less than an arbitrary amount (0 when you are yourself)
            if ((d > 0) && (d < desiredseparation)) 
                // Calculate vector pointing away from neighbor
                let diff = Vector.sub(location, other.location);
                diff.normalize();
                diff.div(d);        // Weight by distance
                steer.add(diff);
                count++;            // Keep track of how many


        // Average -- divide by how many
        if (count > 0) 
            steer.div((float)count);
        

        // As long as the vector is greater than 0
        if (steer.mag() > 0) 
          // First two lines of code below could be condensed with new PVector setMag() method
          // Not using this method until Processing.js catches up
          // steer.setMag(maxspeed);

          // Implement Reynolds: Steering = Desired - Velocity
          steer.normalize();
          steer.mult(maxspeed);
          steer.sub(velocity);
          steer.limit(maxforce);
        
        steer

    // Alignment
    // For every nearby boid in the system, calculate the average velocity
    member this.align (ArrayList<Boid> boids) =
        member this.neighbordist = 50
        member this.sum = Vector(0, 0)
        let mutable this.count = 0
        for (Boid other : boids) 
            float d = Vector.dist(location, other.location);
            if ((d > 0) && (d < neighbordist)) 
                sum.add(other.velocity);
                this.count++;


        if count > 0 then
          sum.div((float)count);
          // First two lines of code below could be condensed with new PVector setMag() method
          // Not using this method until Processing.js catches up
          // sum.setMag(maxspeed);

          // Implement Reynolds: Steering = Desired - Velocity
          sum.normalize();
          sum.mult(maxspeed);
          let steer = Vector.sub(sum, velocity);
          steer.limit(maxforce);

        else 
          Vector(0, 0);


    // Cohesion
    // For the average location (i.e. center) of all nearby boids, calculate steering vector towards that location
    member this.cohesion (ArrayList<Boid> boids) 
        float neighbordist = 50;
        let sum = new Vector(0, 0);   // Start with empty vector to accumulate all locations
        let count = 0;
        for (Boid other : boids) 
            float d = PVector.dist(location, other.location);
            if ((d > 0) && (d < neighbordist)) {
            sum.add(other.location); // Add location
            count++;

        if (count > 0) then
            sum.div(count);
            seek(sum);  // Steer towards the location

        else 
            Vector(0, 0);
