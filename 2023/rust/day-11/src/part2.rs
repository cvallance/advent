// The galaxies are much older (and thus much farther apart) than the researcher initially estimated.
//
// Now, instead of the expansion you did before, make each empty row or column one million times larger. That is, each empty row should be replaced with 1000000 empty rows, and each empty column should be replaced with 1000000 empty columns.
//
// (In the example above, if each empty row or column were merely 10 times larger, the sum of the shortest paths between every pair of galaxies would be 1030. If each empty row or column were merely 100 times larger, the sum of the shortest paths between every pair of galaxies would be 8410. However, your universe will need to expand far beyond these values.)
//
// Starting with the same initial image, expand the universe according to these new rules, then find the length of the shortest path between every pair of galaxies. What is the sum of these lengths?

use crate::custom_error::AocError;
use helpers::Point;
use std::cmp::{max, min};
use std::collections::HashSet;
use tracing::info;

pub fn process(input: &str) -> miette::Result<u64, AocError> {
    // let input = include_str!("../test_input.txt");
    // let input = include_str!("../test_input_two.txt");
    let mut universe: Vec<Vec<char>> = input
        .lines()
        .map(|line| line.chars().collect::<Vec<char>>())
        .collect();

    let (empty_rows, empty_cols) = expand_universe(&mut universe);
    let galaxies = find_galaxies(&universe);

    let mut result = 0;
    for i in 0..galaxies.len() - 1 {
        for j in i + 1..galaxies.len() {
            let distance = find_path(galaxies[i], galaxies[j], &empty_rows, &empty_cols);
            // info!(
            //     "Distance from {:?} to {:?} is {}",
            //     galaxies[i], galaxies[j], distance
            // );
            result += distance;
        }
    }

    Ok(result as u64)
}

const MULTIPLIER: u64 = 1_000_000;

fn find_path(
    one: Point,
    two: Point,
    empty_rows: &HashSet<usize>,
    empty_cols: &HashSet<usize>,
) -> u64 {
    let mut result = 0;

    let min_y = min(one.y, two.y) as usize;
    let max_y = max(one.y, two.y) as usize;
    for y in min_y..max_y {
        if empty_rows.contains(&y) {
            result += MULTIPLIER;
        } else {
            result += 1
        }
    }

    let min_x = min(one.x, two.x) as usize;
    let max_x = max(one.x, two.x) as usize;
    for x in min_x..max_x {
        if empty_cols.contains(&x) {
            result += MULTIPLIER;
        } else {
            result += 1
        }
    }

    result
}

fn find_galaxies(universe: &Vec<Vec<char>>) -> Vec<Point> {
    let mut galaxies = Vec::new();
    for (row, line) in universe.iter().enumerate() {
        for (col, c) in line.iter().enumerate() {
            if *c == '#' {
                galaxies.push(Point::new(col as i32, row as i32));
            }
        }
    }

    galaxies
}

fn expand_universe(universe: &mut Vec<Vec<char>>) -> (HashSet<usize>, HashSet<usize>) {
    let rows_to_expand = universe
        .iter()
        .enumerate()
        .filter(|(_, row)| row.iter().all(|&c| c == '.'))
        .map(|(i, _)| i)
        .collect::<HashSet<usize>>();

    let mut cols_to_expand = HashSet::new();
    for col in 0..universe[0].len() {
        if universe.iter().all(|row| row[col] == '.') {
            cols_to_expand.insert(col);
        }
    }

    (rows_to_expand, cols_to_expand)
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_process() -> miette::Result<()> {
        let input = include_str!("../test_input.txt");
        assert_eq!(82000210, process(input)?);
        Ok(())
    }
}
