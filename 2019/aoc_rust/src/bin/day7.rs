extern crate itertools;

use aoc_rust::intcodes;
use intcodes::IntcodeComputer;
use itertools::Itertools;
use std::collections::VecDeque;

fn main() {
    let base_comp = IntcodeComputer::from_file("day7_input.txt");

    let phase_settings = (0u32..5).permutations(5);
    let mut highest = 0;
    for phase_setting in phase_settings {
        let result = calc_thruster_output(&base_comp, phase_setting);
        if result > highest {
            highest = result;
        }
    }

    println!("part 1 {}", highest);

    let phase_settings = (5u32..10).permutations(5);
    let mut highest = 0;
    for phase_setting in phase_settings {
        let result = calc_thruster_output_loop(&base_comp, phase_setting);
        if result > highest {
            highest = result;
        }
    }

    println!("part 2 {}", highest);
}

fn calc_thruster_output(comp: &IntcodeComputer, phase_setting: Vec<u32>) -> i32 {
    let mut comp_output = 0;
    for value in phase_setting {
        let mut loop_comp = comp.clone();
        loop_comp.inputs.push_back(value as i32);
        loop_comp.inputs.push_back(comp_output);
        loop_comp.process_intcodes();
        comp_output = loop_comp.outputs.pop_front().unwrap();
    }
    comp_output
}

fn calc_thruster_output_loop(comp: &IntcodeComputer, phase_setting: Vec<u32>) -> i32 {
    let mut comp_queue = VecDeque::new();
    for value in phase_setting {
        let mut loop_comp = comp.clone();
        loop_comp.inputs.push_back(value as i32);
        comp_queue.push_back(loop_comp);
    }

    let mut comp_output = 0;
    while let Some(comp) = comp_queue.pop_front().as_mut() {
        if comp.halted {
            break;
        }
        comp.inputs.push_back(comp_output);
        comp.process_intcodes();
        comp_output = comp.outputs.pop_front().unwrap();
        comp_queue.push_back(comp.clone());
    }

    comp_output
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn first_example() {
        let phase_setting = vec![4, 3, 2, 1, 0];
        let test = vec![
            3, 15, 3, 16, 1002, 16, 10, 16, 1, 16, 15, 15, 4, 15, 99, 0, 0,
        ];
        let base_comp = IntcodeComputer::from_vec(test);
        let result = calc_thruster_output(&base_comp, phase_setting);

        assert_eq!(result, 43210);
    }

    #[test]
    fn second_example() {
        let phase_setting = vec![0, 1, 2, 3, 4];
        let test = vec![
            3, 23, 3, 24, 1002, 24, 10, 24, 1002, 23, -1, 23, 101, 5, 23, 23, 1, 24, 23, 23, 4, 23,
            99, 0, 0,
        ];
        let base_comp = IntcodeComputer::from_vec(test);
        let result = calc_thruster_output(&base_comp, phase_setting);

        assert_eq!(result, 54321);
    }

    #[test]
    fn third_example() {
        let phase_setting = vec![1, 0, 4, 3, 2];
        let test = vec![
            3, 31, 3, 32, 1002, 32, 10, 32, 1001, 31, -2, 31, 1007, 31, 0, 33, 1002, 33, 7, 33, 1,
            33, 31, 31, 1, 32, 31, 31, 4, 31, 99, 0, 0, 0,
        ];
        let base_comp = IntcodeComputer::from_vec(test);
        let result = calc_thruster_output(&base_comp, phase_setting);

        assert_eq!(result, 65210);
    }

    #[test]
    fn part_2_first_example() {
        let phase_setting = vec![9, 8, 7, 6, 5];
        let test = vec![
            3, 26, 1001, 26, -4, 26, 3, 27, 1002, 27, 2, 27, 1, 27, 26, 27, 4, 27, 1001, 28, -1,
            28, 1005, 28, 6, 99, 0, 0, 5,
        ];
        let base_comp = IntcodeComputer::from_vec(test);
        let result = calc_thruster_output_loop(&base_comp, phase_setting);

        assert_eq!(result, 139629729);
    }

    #[test]
    fn part_2_second_example() {
        let phase_setting = vec![9, 7, 8, 5, 6];
        let test = vec![
            3, 52, 1001, 52, -5, 52, 3, 53, 1, 52, 56, 54, 1007, 54, 5, 55, 1005, 55, 26, 1001, 54,
            -5, 54, 1105, 1, 12, 1, 53, 54, 53, 1008, 54, 0, 55, 1001, 55, 1, 55, 2, 53, 55, 53, 4,
            53, 1001, 56, -1, 56, 1005, 56, 6, 99, 0, 0, 0, 0, 10,
        ];
        let base_comp = IntcodeComputer::from_vec(test);
        let result = calc_thruster_output_loop(&base_comp, phase_setting);

        assert_eq!(result, 18216);
    }
}
