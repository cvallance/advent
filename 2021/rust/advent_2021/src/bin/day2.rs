use std::fs::File;
use std::io;
use std::io::BufRead;
use std::path::Path;

struct Instruction {
    direction: String,
    distance: u32,
}

fn main() {
    let lines = read_lines("../../inputs/day2.txt").unwrap();
    let lines: Vec<String> = lines.filter_map(Result::ok).collect();
    let mut instructions = Vec::new();
    for line in lines {
        let raw_instruction: Vec<&str> = line.split(" ").collect();
        instructions.push(Instruction {
            direction: raw_instruction[0].to_string(),
            distance: raw_instruction[1].parse().unwrap()
        });
    }

    // First

    let mut horizonatl_pos:u32 = 0;
    let mut depth: u32 = 0;
    for instruction in &instructions {
        match instruction.direction.as_ref() {
            "forward" => {
                horizonatl_pos += instruction.distance;
            }
            "up" => {
                depth -= instruction.distance;
            }
            "down" => {
                depth += instruction.distance;
            }
            _ => {
                panic!("WTF DUDE")
            }
        }
    }

    println!("First {}", horizonatl_pos * depth); // 1604850

    let mut aim: u32 = 0;
    let mut horizonatl_pos:u32 = 0;
    let mut depth: u32 = 0;
    for instruction in &instructions {
        match instruction.direction.as_ref() {
            "forward" => {
                horizonatl_pos += instruction.distance;
                depth += instruction.distance * aim;
            }
            "up" => {
                aim -= instruction.distance;
            }
            "down" => {
                aim += instruction.distance;
            }
            _ => {
                panic!("WTF DUDE")
            }
        }
    }

    println!("Second {}", horizonatl_pos * depth); // 1685186100
}


fn read_lines<P>(filename: P) -> io::Result<io::Lines<io::BufReader<File>>>
    where P: AsRef<Path>, {
    let file = File::open(filename)?;
    Ok(io::BufReader::new(file).lines())
}