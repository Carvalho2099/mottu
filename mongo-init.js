use admin
db = db.getSiblingDB('vehicle_db')

db.createCollection('Users')
db.createCollection('Vehicles')
db.createCollection('VehicleFiles')

db.Users.insertMany([
  {
    email: "admin@acme.com",
    password: "admin123",
    roles: ["vehicle-read", "vehicle-admin"]
  },
  {
    email: "analista@acme.com",
    password: "analista123",
    roles: ["vehicle-read"]
  }
])
