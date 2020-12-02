use std::fs::File;
use std::io::{self, BufRead};
use std::path::Path;

fn main() {
    let (mut wire_one, mut wire_two) = get_wires("day3_input.txt");
    wire_one.compute_path();
    wire_two.compute_path();

    let mut part1_shortest = std::u32::MAX;
    let mut part2_shortest = std::u32::MAX;
    for (point, steps) in &wire_one.path_with_steps {
        if let Some(wire_two_match) = wire_two.path_with_steps.get_key_value(&point) {
            let distance = (point.x.abs() + point.y.abs()) as u32;
            if distance < part1_shortest {
                part1_shortest = distance;
            }

            let steps = steps + wire_two_match.1;
            if steps < part2_shortest {
                part2_shortest = steps;
            }
        }
    }

    println!("part 1 {}", part1_shortest);
    println!("part 2 {}", part2_shortest);
}

#[derive(Hash, PartialEq, Eq, Debug, Copy, Clone)]
struct Point {
    pub x: i32,
    pub y: i32,
}

struct Instruction {
    direction: char,
    distance: i32,
}

struct Wire {
    instructions: Vec<Instruction>,
    path_with_steps: std::collections::HashMap<Point, u32>,
}

impl Wire {
    fn new(instructions: Vec<Instruction>) -> Wire {
        Wire {
            instructions,
            path_with_steps: std::collections::HashMap::new(),
        }
    }

    fn compute_path(&mut self) {
        let mut last = Point { x: 0, y: 0 };
        let mut steps: u32 = 0;
        for instruction in &self.instructions {
            let action: Box<dyn Fn(Point) -> Point> = match instruction.direction {
                'U' => Box::new(|point: Point| Point {
                    x: point.x,
                    y: point.y + 1,
                }),
                'D' => Box::new(|point: Point| Point {
                    x: point.x,
                    y: point.y - 1,
                }),
                'L' => Box::new(|point: Point| Point {
                    x: point.x - 1,
                    y: point.y,
                }),
                'R' => Box::new(|point| Point {
                    x: point.x + 1,
                    y: point.y,
                }),
                _ => panic!("Invalid direction"),
            };

            let mut loops = instruction.distance;
            while loops > 0 {
                last = action(last);
                steps += 1;
                if !self.path_with_steps.contains_key(&last) {
                    self.path_with_steps.insert(last, steps);
                }
                loops -= 1;
            }
        }
    }
}

fn get_wires<P>(filename: P) -> (Wire, Wire)
where
    P: AsRef<Path>,
{
    let file = File::open(filename).unwrap();
    let mut lines = io::BufReader::new(file).lines();
    let first_line = lines.next().unwrap().unwrap();
    let second_line = lines.next().unwrap().unwrap();

    let first_wire = Wire::new(parse_instructions(&first_line));
    let second_wire = Wire::new(parse_instructions(&second_line));

    return (first_wire, second_wire);
}

fn parse_instructions(line: &str) -> Vec<Instruction> {
    let mut values = Vec::new();
    for raw_instruction in line.split(",") {
        let mut chars = raw_instruction.chars();
        let direction = chars.next().unwrap();
        let distance = chars.collect::<String>().parse::<i32>().unwrap();
        values.push(Instruction {
            direction,
            distance,
        });
    }
    values
}
