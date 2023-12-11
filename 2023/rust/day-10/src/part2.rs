use crate::custom_error::AocError;
use crate::shared::{parse_input, Pipe, Point};
use std::collections::HashSet;
use tracing::info;

pub fn process(input: &str) -> miette::Result<u64, AocError> {
    let input = include_str!("../test_input.txt");
    // let input = include_str!("../test_input_four.txt");
    let grid = parse_input(input);

    let mut start = Point::new(0, 0);
    'outer: for (y, row) in grid.iter().enumerate() {
        for (x, pipe) in row.iter().enumerate() {
            if *pipe == Pipe::Start {
                start = Point::new(x as i32, y as i32);
                break 'outer;
            }
        }
    }

    let mut visited = HashSet::new();
    let mut queue = Vec::new();
    let mut left_set = HashSet::new();
    let mut right_set = HashSet::new();
    queue.push((start, 0));
    'outer: while let Some((point, distance)) = queue.pop() {
        let current_pipe = grid[point.y as usize][point.x as usize];
        for next_direction in current_pipe.get_next_directions() {
            let next = point + next_direction;
            info!("CurrPoint: {:?}", point);
            info!("NextPoint: {:?}", next);
            if next.x < 0
                || next.y < 0
                || next.x >= grid[0].len() as i32
                || next.y >= grid.len() as i32
            {
                info!("\tOut of bounds");
                continue;
            }

            let next_pipe = grid[next.y as usize][next.x as usize];
            if next_pipe == Pipe::Start && distance > 1 {
                info!("\tNext is start");
                visited.insert(point);
                queue.push((point, distance));
                break 'outer;
            }
            // If we've already visited this point, ignore it
            if visited.contains(&next) {
                info!("\tAlready visited");
                continue;
            }

            if next_pipe == Pipe::Ground {
                info!("\tGround ignore");
                visited.insert(next);
                continue;
            }

            if next_direction == Point::UP && next_pipe.has_south_connection() {
                info!("\tNext has south connection");
                visited.insert(point);
                queue.push((point, distance));
                queue.push((next, distance + 1));
                break;
            } else if next_direction == Point::DOWN && next_pipe.has_north_connection() {
                info!("\tNext has north connection");
                visited.insert(point);
                queue.push((point, distance));
                queue.push((next, distance + 1));
                break;
            } else if next_direction == Point::LEFT && next_pipe.has_east_connection() {
                info!("\tNext has east connection");
                visited.insert(point);
                queue.push((point, distance));
                queue.push((next, distance + 1));
                break;
            } else if next_direction == Point::RIGHT && next_pipe.has_west_connection() {
                info!("\tNext has west connection");
                visited.insert(point);
                queue.push((point, distance));
                queue.push((next, distance + 1));
                break;
            }

            info!("\tNo good");
        }
    }

    info!("Result: {}", result);
    Ok(result / 2)
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_process() -> miette::Result<()> {
        let input = include_str!("../test_input_four.txt");
        assert_eq!(10, process(input)?);
        Ok(())
    }
}
